using DirectoryService.Application.Tasks;
using DirectoryService.Domain.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Tasks;

public class CleanupLocationsTask: ICleanupTask
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<CleanupLocationsTask> _logger;
    public string Key => ApplicationConstants.CLEANUP_KEYS.LOCATIONS;

    public CleanupLocationsTask(DirectoryServiceDbContext dbContext, ILogger<CleanupLocationsTask> logger)
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
            _logger.LogError(e, "Fail to delete Positions");
            return 0;
        }
    }
}