using DirectoryService.Domain.Positions;

namespace DirectoryService.Application.Positions;

public interface IPositionRepository
{
    Task<Guid> AddAsync(Position position, CancellationToken cancellationToken);
    
    Task<Guid> SaveAsync(Position position, CancellationToken cancellationToken);
    
    Task<Guid> DeleteAsync(Guid positionId, CancellationToken cancellationToken);
    
    Task<Guid> GetByIdAsync(Guid positionId, CancellationToken cancellationToken);

}