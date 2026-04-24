using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Departments;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using General.Errors;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Departments;

public class CreateDepartmentHandler : ICommandHandler<Guid, CreateDepartmentCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly ILogger<CreateDepartmentHandler> _logger;
    private readonly IValidator<CreateDepartmentRequest> _validator;

    public CreateDepartmentHandler(
        IDepartmentsRepository departmentsRepository,
        ILocationsRepository locationsRepository,
        ILogger<CreateDepartmentHandler> logger,
        IValidator<CreateDepartmentRequest> validator)
    {
        _departmentsRepository = departmentsRepository;
        _locationsRepository = locationsRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, FailList>> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        // Валидация входных параметров
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToFailList();
        
        var name = DepartmentName.Create(command.Request.Name);
        var identifier = Identifier.Create(command.Request.Identifier);
        Department? parent = null;
        
        // Бизнес валдидация
        if (!await _locationsRepository.AllLocationsExistAsync(command.Request.LocationIds, cancellationToken))
            return Failure.NotFoundCollectionEntity($"One or more locations not found : {string.Join(',', command.Request.LocationIds)}", "locations.not.found").ToFailList();

        if (command.Request.ParentId != null)
        {
            var searchParent =
                await _departmentsRepository.GetByIdAsync(command.Request.ParentId.Value, cancellationToken);

            if (searchParent.IsFailure)
                return searchParent.Error.ToFailList();

            parent = searchParent.Value;
        }
        
        // Coздание доменных моделей
        Guid departmentID = Guid.NewGuid();
        
        List<DepartmentLocation> departmentLocations = command.Request.LocationIds
            .Select(id => new DepartmentLocation(departmentID, id)).ToList();

        var department = parent == null
            ? Department.CreateParent(name.Value, identifier.Value, departmentLocations, departmentID)
            : Department.CreateChild(name.Value, identifier.Value, parent, departmentLocations, departmentID);

        if (department.IsFailure)
            return department.Error;
        
        // Сохранение доменных моделей в БД
        var adding = await _departmentsRepository.AddAsync(department.Value, cancellationToken);

        if (adding.IsFailure)
            return adding.Error.ToFailList();

        return departmentID;

    }
}