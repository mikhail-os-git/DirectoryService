using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Application.Positions.Interfaces;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Departments.DetachPosition;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Departments;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments.DetachPosition;

public class DetachPositionHandler: ICommandHandler<DetachPositionResponse, DetachPositionCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly IPositionsRepository _positionsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<DetachPositionCommand> _validator;

    public DetachPositionHandler(IDepartmentsRepository departmentsRepository, IPositionsRepository positionsRepository, ITransactionManager transactionManager, IValidator<DetachPositionCommand> validator)
    {
        _departmentsRepository = departmentsRepository;
        _positionsRepository = positionsRepository;
        _transactionManager = transactionManager;
        _validator = validator;
    }
    
    public async Task<Result<DetachPositionResponse, FailList>> Handle(
        DetachPositionCommand command,
        CancellationToken cancellationToken)
    {
        var validate = await _validator.ValidateAsync(command, cancellationToken);

        if (!validate.IsValid)
            return validate.ToFailList();
        
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

        int delete = await _departmentsRepository.DetachPosition(search, cancellationToken);
        
        if(delete == 0)
            return Failure.NotFoundEntity("Position is not detached to department", command.PositionId, "department-position.not.found").ToFailList();

        var save = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (save.IsFailure)
            return save.Error.ToFailList();

        return new DetachPositionResponse(command.DepartmentId, command.PositionId);
    }
}