using CSharpFunctionalExtensions;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.ValueObjects;
using General.Errors;

namespace DirectoryService.Application.Positions.Interfaces;

public interface IPositionsRepository
{
    Task<Result<Guid, Failure>> AddAsync(Position position, CancellationToken cancellationToken);
    
    Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken);

    Task<bool> ActivePositionWithNameExistsAsync(PositionName name, CancellationToken cancellationToken);
    
    // Task<Guid> DeleteAsync(Guid positionId, CancellationToken cancellationToken);
    
    // Task<Guid> GetByIdAsync(Guid positionId, CancellationToken cancellationToken);
}