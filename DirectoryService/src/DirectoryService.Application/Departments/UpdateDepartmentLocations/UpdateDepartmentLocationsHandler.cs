using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Application.Validation;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Departments;
using FluentValidation;
using General.Errors;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Departments.UpdateDepartmentLocations;

public class UpdateDepartmentLocationsHandler : ICommandHandler<Guid, UpdateDepartmentLocationsCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<UpdateDepartmentLocationsCommand> _validator;
    private readonly ILogger<UpdateDepartmentLocationsHandler> _logger;

    public UpdateDepartmentLocationsHandler(
        IDepartmentsRepository departmentsRepository,
        ILocationsRepository locationsRepository,
        ITransactionManager transactionManager,
        IValidator<UpdateDepartmentLocationsCommand> validator,
        ILogger<UpdateDepartmentLocationsHandler> logger)
    {
        _departmentsRepository = departmentsRepository;
        _locationsRepository = locationsRepository;
        _transactionManager = transactionManager;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<Guid, FailList>> Handle(
        UpdateDepartmentLocationsCommand command,
        CancellationToken cancellationToken)
    {
        // Валидация входных параметров
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToFailList();
        
        // Создание транзакции
        var createScope = await _transactionManager.BeginTransactionAsync(cancellationToken);
        if (createScope.IsFailure)
            return createScope.Error.ToFailList();

        await using var transaction = createScope.Value;

        // Бизнес валидация
        bool allLocationsExist = await _locationsRepository.AllMatchAsync(
            command.LocationIds,
            l => command.LocationIds.Contains(l.Id), 
            cancellationToken);

        if (!allLocationsExist)
            return LocationErrors.CollectionNotFound(command.LocationIds).ToFailList();

        Department? department = await 
            _departmentsRepository.GetByAsync(d => d.Id == command.DepartmentId, cancellationToken);
        
        if(department is null)
            return DepartmentErrors.NotFound(command.DepartmentId).ToFailList();
        
        // Обновление локаций
        var delete =
            await _departmentsRepository.DeleteDepartmentLocationsByIdAsync(command.DepartmentId, cancellationToken);

        if (delete.IsFailure)
        {
            await transaction.RollbackAsync(cancellationToken);
            return delete.Error.ToFailList();
        }

        department.UpdateLocations(command.LocationIds);
        
        var saveChanges = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (saveChanges.IsFailure)
        {
            await transaction.RollbackAsync(cancellationToken);
            return saveChanges.Error.ToFailList();
        }
        
        var commit = await transaction.CommitAsync(cancellationToken);
        
        if (commit.IsFailure)
            return commit.Error.ToFailList();

        return command.DepartmentId;
    }
}
