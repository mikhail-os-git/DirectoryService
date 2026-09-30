using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;
using General.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Repositories;

public class LocationsRepository: ILocationsRepository
{
    private readonly DirectoryServiceDbContext _context;
    private readonly ILogger<LocationsRepository> _logger;

    public LocationsRepository(DirectoryServiceDbContext context, ILogger<LocationsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(Location location, CancellationToken cancellationToken)
    {
      await _context.Locations.AddAsync(location, cancellationToken);
      return location.Id;
    }

    public async Task<Location?> GetByAsync(Expression<Func<Location, bool>> expression, CancellationToken cancellationToken)
    {
        return await _context.Locations.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<bool> IsMatchAsync(
        Expression<Func<Location, bool>> expression,
        CancellationToken cancellationToken)
    {
        return await _context.Locations.AnyAsync(expression, cancellationToken);
    }

    public async Task<bool> AllMatchAsync(IEnumerable<Guid> ids, Expression<Func<Location, bool>> expression, CancellationToken cancellationToken)
    {
        List<Guid> collection = ids.ToList();
        int foundCount = await _context.Locations
            .Where(expression)
            .CountAsync(cancellationToken);

        return foundCount == collection.Count;
    }

    public async Task<Result<int, Failure>> HardDeleteAsync(
        Expression<Func<Location, bool>> expression,
        CancellationToken cancellationToken, bool save = false)
    {
        int delete = await _context.Locations.
            IgnoreQueryFilters()
            .Where(expression)
            .ExecuteDeleteAsync(cancellationToken);
        
        if (!save)
            return delete;
        
        var saveChanges = await SaveAsync(cancellationToken);
        
        return saveChanges.IsFailure ? saveChanges.Error : delete;
    }

    private async Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken) 
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Failure>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to save changes: {Error}", ex);
            return UnitResult.Failure(CommonErrors.InternalError);
        }
    }
}