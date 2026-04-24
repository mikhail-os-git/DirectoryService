using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;
using General.Errors;

namespace DirectoryService.Application.Locations.Interfaces;

public interface ILocationsRepository
{
    Task<Result<Guid, Failure>> AddAsync(Location location, CancellationToken cancellationToken);

    Task<bool> LocationNameExistsAsync(LocationName name, CancellationToken cancellationToken);

    Task<bool> LocationAddressExistsAsync(Address address, CancellationToken cancellationToken);
    
    Task<bool> AllLocationsExistAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    
    Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken);
    
    // Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken);
    //
    // Task<Guid> GetByIdAsync(Guid locationId, CancellationToken cancellationToken);
}