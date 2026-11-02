using DirectoryService.Application.Tasks;
using DirectoryService.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DirectoryService.Infrastructure.Workers;

public class DeleteBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly DeleteWorkerOptions _options;
    private readonly ILogger<DeleteBackgroundService> _logger;

    public DeleteBackgroundService(IServiceScopeFactory scopeFactory, IOptions<DeleteWorkerOptions> options, ILogger<DeleteBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }
    
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_options.Interval);

        do
        {
            try
            {
                await RunCleanupAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Cleanup iteration failed");
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }
    
    private async Task RunCleanupAsync(CancellationToken ct)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();

        foreach (var task in scope.ServiceProvider.GetServices<ICleanupTask>())
        {
            if (!_options.Tables.TryGetValue(task.Key, out var table))
            {
                _logger.LogWarning("No cleanup config for {Key}, skipped", task.Key);
                continue;
            }

            try
            {
                var cutoff = TimeProvider.System.GetUtcNow() - table.RetentionPeriod;
                var total = 0;
                int deleted;
                do
                {
                    deleted = await task.DeleteBatchAsync(cutoff, _options.BatchSize, ct);
                    total += deleted;
                }
                while (deleted == _options.BatchSize);

                if (total > 0)
                    _logger.LogInformation("{Key}: deleted {Count} records", task.Key, total);
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                _logger.LogError(e, "Cleanup failed for {Key}", task.Key);
            }
        }
    }
}