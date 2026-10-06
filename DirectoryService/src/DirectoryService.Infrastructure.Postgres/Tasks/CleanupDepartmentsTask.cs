using DirectoryService.Application.Database;
using DirectoryService.Application.Tasks;
using DirectoryService.Domain.Common.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Tasks;

public class CleanupDepartmentsTask: ICleanupTask
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<CleanupDepartmentsTask> _logger;
    public string Key => ApplicationConstants.CLEANUP_KEYS.DEPARTMENTS;

    public CleanupDepartmentsTask(DirectoryServiceDbContext dbContext, ILogger<CleanupDepartmentsTask> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<int> DeleteBatchAsync(DateTimeOffset olderThan, int batchSize, CancellationToken cancellationToken)
    {
        try
        {
            int deleted = await _dbContext.Departments
                .IgnoreQueryFilters()
                .Where(d => 
                    !d.IsActive &&
                    d.ParentId == null &&
                    (d.DeletedAt != null &&
                     d.DeletedAt < olderThan))
                .Where(d => !_dbContext.Departments
                    .IgnoreQueryFilters()
                    .Any(c => c.ParentId == d.Id))
                .OrderBy(d => d.Id)
                .Take(batchSize)
                .ExecuteDeleteAsync(cancellationToken);

            return deleted;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to delete Departments");
            return 0;
        }
    }
}