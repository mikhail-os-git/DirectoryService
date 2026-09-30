using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Application.Positions.Interfaces;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Departments.AttachPosition;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Departments;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments.AttachPosition;

public class AttachPositionHandler : ICommandHandler<AttachPositionResponse, AttachPositionCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly IPositionsRepository _positionsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<AttachPositionCommand> _validator;

    public AttachPositionHandler(
        IDepartmentsRepository departmentsRepository, 
        IPositionsRepository positionsRepository,
        ITransactionManager transactionManager, 
        IValidator<AttachPositionCommand> validator)
    {
        _departmentsRepository = departmentsRepository;
        _positionsRepository = positionsRepository;
        _transactionManager = transactionManager;
        _validator = validator;
    }
    
    public async Task<Result<AttachPositionResponse, FailList>> Handle(
        AttachPositionCommand command,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(command, cancellationToken);

        if (!validation.IsValid)
            return validation.ToFailList();
        
        bool departmentExist =
            await _departmentsRepository.IsMatchAsync(d => d.Id == command.DepartmentId, cancellationToken);

        bool positionExist =
            await _positionsRepository.IsMatchAsync(p => p.Id == command.PositionId, cancellationToken);
        
        if (!departmentExist)
            return DepartmentErrors.NotFound(command.DepartmentId).ToFailList();

        if (!positionExist)
            return PositionErrors.NotFound(command.PositionId).ToFailList();

        Expression<Func<DepartmentPosition, bool>> search = dp => dp.DepartmentId == command.DepartmentId
                                                                      && dp.PositionId == command.PositionId;
        
        bool isAlreadyAttached = await _departmentsRepository.CheckAttachedPosition(search, cancellationToken);

        if (isAlreadyAttached)
            return Failure.Conflict("Position is already attached to the department", "department-position.conflict.already-attached").ToFailList();

        await _departmentsRepository.AttachPosition(command.DepartmentId, command.PositionId, cancellationToken);

        var save = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (save.IsFailure)
            return save.Error.ToFailList();

        return new AttachPositionResponse(command.DepartmentId, command.PositionId);
    }
}