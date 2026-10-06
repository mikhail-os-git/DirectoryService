using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Locations;
using General.Errors;

namespace DirectoryService.Application.Locations.SoftDeleteLocation;

public class SoftDeleteLocationHandler: ICommandHandler<Guid, SoftDeleteLocationCommand>
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly ITransactionManager _transactionManager;

    public SoftDeleteLocationHandler(ILocationsRepository locationsRepository, ITransactionManager transactionManager)
    {
        _locationsRepository = locationsRepository;
        _transactionManager = transactionManager;
    }
    
    public async Task<Result<Guid, FailList>> Handle(SoftDeleteLocationCommand command, CancellationToken cancellationToken)
    {
        if (command.LocationId == Guid.Empty)
            return Failure.Validation("Location Id can't be Empty", "location-id.invalid.empty").ToFailList();

        Location? location = await _locationsRepository.GetByAsync(l => l.Id == command.LocationId, cancellationToken);

        if (location is null)
            return LocationErrors.NotFound(command.LocationId).ToFailList();
        
        location.Delete();

        var save = await _transactionManager.SaveChangesAsync(cancellationToken);
        
        if (save.IsFailure)
            return save.Error.ToFailList();

        return command.LocationId;
    }
}