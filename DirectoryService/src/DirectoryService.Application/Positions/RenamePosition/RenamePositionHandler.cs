using CSharpFunctionalExtensions;
using DirectoryService.Application.Positions.Interfaces;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Shared.Core.Abstractions;
using Shared.Core.Database;
using Shared.Core.Validation;
using Shared.Kernal.Failures;

namespace DirectoryService.Application.Positions.RenamePosition;

public class RenamePositionHandler: ICommandHandler<Guid, RenamePositionCommand>
{
    private readonly IPositionsRepository _positionsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<RenamePositionCommand> _validator;

    public RenamePositionHandler(IPositionsRepository positionsRepository, ITransactionManager transactionManager, IValidator<RenamePositionCommand> validator)
    {
        _positionsRepository = positionsRepository;
        _transactionManager = transactionManager;
        _validator = validator;
    }
    
    public async Task<Result<Guid, FailList>> Handle(RenamePositionCommand command, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(command, cancellationToken);

        if (!validation.IsValid)
            return validation.ToFailList();
        
        Position? position = await _positionsRepository.GetByAsync(
            p => p.Id == command.PositionId, 
            cancellationToken);

        if (position is null)
            return PositionErrors.NotFound(command.PositionId).ToFailList();

        var name = PositionName.Create(command.NewName);

        if (name.IsFailure)
            return name.Error.ToFailList();
        
        var rename = position.Rename(name.Value);

        if (rename.IsFailure)
            return rename.Error.ToFailList();
        
        var save = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (save.IsFailure)
        {
            return save.Error.ToFailList();
        }

        return position.Id;
    }
}