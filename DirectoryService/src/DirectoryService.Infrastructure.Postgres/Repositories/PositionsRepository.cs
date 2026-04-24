using CSharpFunctionalExtensions;
using DirectoryService.Application.Positions;
using DirectoryService.Application.Positions.Interfaces;
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
    
    public async Task<Result<Guid, Failure>> AddAsync(Position position, CancellationToken cancellationToken)
    {
        await _context.Positions.AddAsync(position, cancellationToken);

        var saving = await SaveAsync(cancellationToken);

        if (saving.IsFailure)
            return saving.Error;

        _logger.LogInformation("Position {Id} created successfully", position.Id);
        
        return position.Id;
    }

    public async Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Failure>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to save changes: {Error}", ex);
            return UnitResult.Failure(Failure.Error("Something went wrong", "server.internal"));
        }
    }

    public async Task<bool> ActivePositionWithNameExistsAsync(PositionName name, CancellationToken cancellationToken)
    {
        return await _context.Positions.AnyAsync(p => p.PositionName == name && p.IsActive, cancellationToken);
    }
}