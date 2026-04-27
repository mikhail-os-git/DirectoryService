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
        
        Department? parent = command.Request.ParentId is not null
            ? await _departmentsRepository.GetByAsync(d => d.Id == command.Request.ParentId.Value, cancellationToken)
            : null;

        // Бизнес валдидация
        if (command.Request.ParentId is not null && parent is null)
        {
            return Failure
                .NotFoundEntity("Department not found", command.Request.ParentId.Value, "department.not.found")
                .ToFailList();
        }

        bool allLocationExist = await _locationsRepository.AllMatchAsync(
            command.Request.LocationIds,
            l => command.Request.LocationIds.Contains(l.Id),
            cancellationToken);
        
        if (!allLocationExist)
            return Failure.NotFoundCollectionEntity($"One or more locations not found : {string.Join(',', command.Request.LocationIds)}", "locations.not.found").ToFailList();
        
        // Coздание доменных моделей
        Guid departmentID = Guid.NewGuid();
        
        List<DepartmentLocation> departmentLocations = command.Request.LocationIds
            .Select(id => new DepartmentLocation(departmentID, id)).ToList();

        var department = parent is null
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