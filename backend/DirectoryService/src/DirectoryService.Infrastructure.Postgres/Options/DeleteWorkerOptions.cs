namespace DirectoryService.Infrastructure.Options;

public record DeleteWorkerOptions
{
    public const string NAME = "DeleteWorker";
    public TimeSpan Interval { get; init; } = TimeSpan.FromMinutes(20);
    
    public Dictionary<string, TableOptions> Tables { get; init; } = [];
    public int BatchSize { get; init; } = 200;
}

public sealed class TableOptions
{
    public TimeSpan RetentionPeriod { get; init; } = TimeSpan.FromDays(30);
}