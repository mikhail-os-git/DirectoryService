using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Positions;
using DirectoryService.Application.Positions.Interfaces;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.ValueObjects;
using General.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Repositories;

public class PositionsRepository: IPositionsRepository
{
    private readonly DirectoryServiceDbContext _context;
    private readonly ILogger<PositionsRepository> _logger;

    public PositionsRepository(DirectoryServiceDbContext context, ILogger<PositionsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Guid> AddAsync(Position position, CancellationToken cancellationToken)
    {
        await _context.Positions.AddAsync(position, cancellationToken);
        return position.Id;
    }

    public async Task<Position?> GetByAsync(
        Expression<Func<Position, bool>> expression,
        CancellationToken cancellationToken,
        bool includeInactive = false)
    {
        IQueryable<Position> query = _context.Positions;

        if (includeInactive)
            query = query.IgnoreQueryFilters();
            
        return await query.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<bool> IsMatchAsync(Expression<Func<Position, bool>> expression, CancellationToken cancellationToken)
    {
        return await _context.Positions.AnyAsync(expression, cancellationToken);
    }

    public async Task<bool> AllMatchAsync(IEnumerable<Guid> ids, Expression<Func<Position, bool>> expression,
        CancellationToken cancellationToken)
    {
        List<Guid> collection = ids.ToList();

        int count = await _context.Positions.Where(expression).CountAsync(cancellationToken);

        return count == collection.Count;
    }

    public async Task<Result<int, Failure>> HardDeleteAsync(
        Expression<Func<Position, bool>> expression,
        CancellationToken cancellationToken, bool save = false)
    {
        int delete = await _context.Positions
            .IgnoreQueryFilters()
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