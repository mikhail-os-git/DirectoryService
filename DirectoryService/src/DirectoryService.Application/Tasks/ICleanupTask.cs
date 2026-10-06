namespace DirectoryService.Application.Tasks;

public interface ICleanupTask
{
    string Key { get; }
    Task<int> DeleteBatchAsync(DateTimeOffset olderThan, int batchSize, CancellationToken cancellationToken);
}