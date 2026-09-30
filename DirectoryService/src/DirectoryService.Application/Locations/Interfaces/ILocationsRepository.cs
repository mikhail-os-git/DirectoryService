using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;
using General.Errors;

namespace DirectoryService.Application.Locations.Interfaces;

public interface ILocationsRepository
{
    Task<Guid> AddAsync(Location location, CancellationToken cancellationToken);
    
    Task<Location?> GetByAsync(Expression<Func<Location, bool>> expression, CancellationToken cancellationToken);
    
    Task<bool> IsMatchAsync(Expression<Func<Location, bool>> expression, CancellationToken cancellationToken);
    
    Task<bool> AllMatchAsync(IEnumerable<Guid> ids, Expression<Func<Location, bool>> expression, CancellationToken cancellationToken);
    
    Task<Result<int, Failure>> HardDeleteAsync(Expression<Func<Location, bool>> expression, CancellationToken cancellationToken, bool save = false);
    
    // Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken);
}