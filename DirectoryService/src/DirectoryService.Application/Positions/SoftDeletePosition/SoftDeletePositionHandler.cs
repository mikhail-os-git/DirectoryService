using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Positions.Interfaces;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Positions;
using General.Errors;

namespace DirectoryService.Application.Positions.SoftDeletePosition;

public class SoftDeletePositionHandler: ICommandHandler<Guid, SoftDeletePositionCommand>
{
    private readonly IPositionsRepository _positionsRepository;
    private readonly ITransactionManager _transactionManager;

    public SoftDeletePositionHandler(IPositionsRepository positionsRepository, ITransactionManager transactionManager)
    {
        _positionsRepository = positionsRepository;
        _transactionManager = transactionManager;
    }
    
    public async Task<Result<Guid, FailList>> Handle(SoftDeletePositionCommand command, CancellationToken cancellationToken)
    {
        if (command.PositionId == Guid.Empty)
            return Failure.Validation("Position Id can't be Empty", "position-id.invalid.empty").ToFailList();

        Position? position = await _positionsRepository.GetByAsync(p => p.Id == command.PositionId, cancellationToken);

        if (position is null)
            return PositionErrors.NotFound(command.PositionId).ToFailList();

        position.Delete();

        var save = await _transactionManager.SaveChangesAsync(cancellationToken);
        
        if (save.IsFailure)
            return save.Error.ToFailList();
        
        return command.PositionId;
    }
}