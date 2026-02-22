using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations;

namespace DirectoryService.Application.Locations.Interfaces;

public interface ILocationsRepository
{
    Task<Result<Guid, string>> AddAsync(Location location, CancellationToken cancellationToken);
    
    // Task<Guid> SaveAsync(Location location, CancellationToken cancellationToken);
    //
    // Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken);
    //
    // Task<Guid> GetByIdAsync(Guid locationId, CancellationToken cancellationToken);
}