using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace DirectoryService.Infrastructure.Database;

public sealed class NpgSqlConnectionFactory : IDisposable, IAsyncDisposable, IDbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly ILoggerFactory _loggerFactory;
    private bool _disposed;
    
    public NpgSqlConnectionFactory(IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DirectoryServiceDB")!);
        _loggerFactory = CreateLoggerFactory();
        
        dataSourceBuilder.UseLoggerFactory(_loggerFactory);
        
        _dataSource = dataSourceBuilder.Build();
        
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        return await _dataSource.OpenConnectionAsync(cancellationToken);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _dataSource.Dispose();
        _loggerFactory.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;
        await _dataSource.DisposeAsync();
        _loggerFactory.Dispose();
    }

    private ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder => { builder.AddConsole(); });
    
}