using DirectoryService.Application.Tasks;
using DirectoryService.Domain.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Tasks;

public class CleanupPositionsTask : ICleanupTask
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<CleanupPositionsTask> _logger;
    public string Key => ApplicationConstants.CLEANUP_KEYS.POSITIONS;

    public CleanupPositionsTask(DirectoryServiceDbContext dbContext, ILogger<CleanupPositionsTask> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<int> DeleteBatchAsync(DateTimeOffset olderThan, int batchSize, CancellationToken cancellationToken)
    {
        try
        {
            int deleted = await _dbContext.Positions
                .Where(p => !p.IsActive && (p.DeletedAt != null && p.DeletedAt < olderThan))
                .OrderBy(p => p.Id)
                .Take(batchSize)
                .ExecuteDeleteAsync(cancellationToken);

            return deleted;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to delete Positons");
            return 0;
        }
    }
}