using System.Data;
using DirectoryService.Domain.Common.Constants;
using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.Database;
using DirectoryService.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace DirectoryService.IntegrationTests;

public class DirectoryTestWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage", 
        "CA2213:Disposable fields should be disposed",
        Justification = "Disposed in IAsyncLifetime.DisposeAsync")]
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres")
        .WithDatabase("ds_tests_db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage", 
        "CA2213:Disposable fields should be disposed",
        Justification = "Disposed in IAsyncLifetime.DisposeAsync")]
    private NpgsqlConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DirectoryServiceDbContext>();
        
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await _dbConnection.OpenAsync();
        await InitializeRespawnerAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await _dbConnection.CloseAsync();
        await _dbConnection.DisposeAsync();

        await DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"ConnectionStrings:{DatabaseConstants.DATABASE}"] = _dbContainer.GetConnectionString()
            });
        });
    }
    
    public async Task ResetDataBaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public async Task SeedDataBaseAsync()
    { 
        await using var scope = Services.CreateAsyncScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DirectoryTestWebFactory>>();

        try
        {
            await seeder.SeedAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to seeding in tests");
        }
    }

    private async Task InitializeRespawnerAsync()
    {
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions()
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }
    
}