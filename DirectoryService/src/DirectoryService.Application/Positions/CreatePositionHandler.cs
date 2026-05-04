using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Application.Positions.Interfaces;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Positions;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using General.Errors;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Positions;

public class CreatePositionHandler : ICommandHandler<Guid, CreatePositionCommand>
{
    private readonly IPositionsRepository _positionsRepository;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<CreatePositionHandler> _logger;
    private readonly IValidator<CreatePositionRequest> _validator;

    public CreatePositionHandler(
        IPositionsRepository positionsRepository,
        IDepartmentsRepository departmentsRepository,
        ITransactionManager transactionManager,
        ILogger<CreatePositionHandler> logger,
        IValidator<CreatePositionRequest> validator) 
    {
        _positionsRepository = positionsRepository;
        _departmentsRepository = departmentsRepository;
        _transactionManager = transactionManager;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, FailList>> Handle(CreatePositionCommand command, CancellationToken cancellationToken)
    {
        // Валидация входных параметров
        var validateResult = await _validator.ValidateAsync(command.Request, cancellationToken);

        if (!validateResult.IsValid)
            return validateResult.ToFailList();

        var name = PositionName.Create(command.Request.Name);

        // Бизнес валидация
        bool activePosWithNameExist = await _positionsRepository.IsMatchAsync(
            p => p.PositionName == name.Value && p.IsActive,
            cancellationToken);
        
        if (activePosWithNameExist)
            return PositionErrors.PropertyConflict(name.Value.Value, "Name").ToFailList();

        bool allDepartmentsExist = await _departmentsRepository.AllMatchAsync(
            command.Request.DepartmentIds,
            d => command.Request.DepartmentIds.Contains(d.Id), 
            cancellationToken);
        
        if(!allDepartmentsExist)
            return DepartmentErrors.CollectionNotFound(command.Request.DepartmentIds).ToFailList();

        bool allDepartmentsActive = await _departmentsRepository.AllMatchAsync(
            command.Request.DepartmentIds,
            d => command.Request.DepartmentIds.Contains(d.Id) && d.IsActive,
            cancellationToken);
        
        if (!allDepartmentsActive)
            return DepartmentErrors.CollectionInactive(command.Request.DepartmentIds).ToFailList();
        
        // Coздание доменных моделей
        Guid positionId = Guid.NewGuid();
        List<DepartmentPosition> departmentPositions =
            command.Request.DepartmentIds.Select(id => new DepartmentPosition(id, positionId)).ToList();
        
        var position = Position.Create(name.Value, departmentPositions, command.Request.Description, positionId);

        if (position.IsFailure)
            return position.Error.ToFailList();

        // Сохранение доменных моделей в БД
        var id = await _positionsRepository.AddAsync(position.Value, cancellationToken);

        var saving = await _transactionManager.SaveChangesAsync(cancellationToken);
        if (saving.IsFailure)
            return saving.Error.ToFailList();

        return id;
    }
}