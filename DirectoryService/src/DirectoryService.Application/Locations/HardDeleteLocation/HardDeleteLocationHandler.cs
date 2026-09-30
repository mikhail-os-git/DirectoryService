using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Locations;
using General.Errors;

namespace DirectoryService.Application.Locations.HardDeleteLocation;

public class HardDeleteLocationHandler : ICommandHandler<Guid, HardDeleteLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly ITransactionManager _transactionManager;

    public HardDeleteLocationHandler(ILocationsRepository locationsRepository, ITransactionManager transactionManager)
    {
        _locationsRepository = locationsRepository;
        _transactionManager = transactionManager;
    }
    
    public async Task<Result<Guid, FailList>> Handle(HardDeleteLocationCommand command, CancellationToken cancellationToken)
    {
        if (command.LocationId == Guid.Empty)
            return Failure.Validation("Location Id can't be Empty", "location-id.invalid.empty").ToFailList();

        var scope = await _transactionManager.BeginTransactionAsync(cancellationToken);
        if (scope.IsFailure)
            return scope.Error.ToFailList();

        await using var transaction = scope.Value;

        var delete = await _locationsRepository.HardDeleteAsync(
            p => p.Id == command.LocationId, cancellationToken);

        if (delete.IsFailure)
        {
            await transaction.RollbackAsync(cancellationToken);
            return delete.Error.ToFailList();
        }

        if (delete.Value == 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            return LocationErrors.NotFound(command.LocationId).ToFailList();
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

        return command.LocationId;

    }
}