using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.ValueObjects;
using General.Errors;

namespace DirectoryService.Application.Positions.Interfaces;

public interface IPositionsRepository
{
    Task<Result<Guid, Failure>> AddAsync(Position position, CancellationToken cancellationToken);

    Task<Position?> GetByAsync(Expression<Func<Position, bool>> expression, CancellationToken cancellationToken);

    Task<bool> IsMatchAsync(Expression<Func<Position, bool>> expression, CancellationToken cancellationToken);

    Task<bool> AllMatchAsync(IEnumerable<Guid> ids, Expression<Func<Position, bool>> expression,
        CancellationToken cancellationToken);
    
    Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken);
      
    // Task<Guid> DeleteAsync(Guid positionId, CancellationToken cancellationToken);
    
    // Task<Guid> GetByIdAsync(Guid positionId, CancellationToken cancellationToken);
}