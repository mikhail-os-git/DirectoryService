using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Application.Locations.Interfaces;

public interface ILocationsRepository
{
    Task<Guid> AddAsync(Location location, CancellationToken cancellationToken);

    Task<bool> LocationNameExistsAsync(LocationName name, CancellationToken cancellationToken);

    Task<bool> LocationAddressExistsAsync(Address address, CancellationToken cancellationToken);
    
    // Task<Guid> SaveAsync(Location location, CancellationToken cancellationToken);
    //
    // Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken);
    //
    // Task<Guid> GetByIdAsync(Guid locationId, CancellationToken cancellationToken);
}