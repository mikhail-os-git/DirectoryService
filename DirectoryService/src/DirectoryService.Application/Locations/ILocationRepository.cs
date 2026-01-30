using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;

namespace DirectoryService.Application.Locations;

public interface ILocationRepository
{
    Task<Guid> AddAsync(Location location, CancellationToken cancellationToken);
    
    Task<Guid> SaveAsync(Location location, CancellationToken cancellationToken);
    
    Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken);
    
    Task<Guid> GetByIdAsync(Guid locationId, CancellationToken cancellationToken);
}