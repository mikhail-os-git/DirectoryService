using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Positions.Interfaces;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Positions;
using General.Errors;

namespace DirectoryService.Application.Positions.HardDeletePosition;

public class HardDeletePositionHandler : ICommandHandler<Guid, HardDeletePositionCommand>
{
    private readonly IPositionsRepository _positionsRepository;
    private readonly ITransactionManager _transactionManager;

    public HardDeletePositionHandler(IPositionsRepository positionsRepository, ITransactionManager transactionManager)
    {
        _positionsRepository = positionsRepository;
        _transactionManager = transactionManager;
    }
    
    public async Task<Result<Guid, FailList>> Handle(HardDeletePositionCommand command, CancellationToken cancellationToken)
    {
        if (command.PositionId == Guid.Empty)
            return Failure.Validation("Position Id can't be Empty", "position-id.invalid.empty").ToFailList();

        var scope = await _transactionManager.BeginTransactionAsync(cancellationToken);
        if (scope.IsFailure)
            return scope.Error.ToFailList();

        await using var transaction = scope.Value;

        var delete = await _positionsRepository.HardDeleteAsync(
            p => p.Id == command.PositionId, cancellationToken);

        if (delete.IsFailure)
        {
            await transaction.RollbackAsync(cancellationToken);
            return delete.Error.ToFailList();
        }

        if (delete.Value == 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            return PositionErrors.NotFound(command.PositionId).ToFailList();
        }

        var saveChanges = await _transactionManager.SaveChangesAsync(cancellationToken);
        if (saveChanges.IsFailure)
        {
            await transaction.RollbackAsync(cancellationToken);
            return saveChanges.Error.ToFailList();
        }

        var commit = await transaction.CommitAsync(cancellationToken);
        if (commit.IsFailure)
            return commit.Error.ToFailList();

        return command.PositionId;
    }
}