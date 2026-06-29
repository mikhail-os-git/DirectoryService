using DirectoryService.Application.Abstractions;
using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DirectoryService.IntegrationTests;

public class DirectoryBaseTests: IClassFixture<DirectoryTestWebFactory>, IAsyncLifetime
{
    private IServiceProvider _services;
    private Func<Task> _resetDatabase;
    private Func<Task> _seedDatabase;
    public DirectoryBaseTests(DirectoryTestWebFactory factory)
    {
        _services = factory.Services;
        _resetDatabase = factory.ResetDataBaseAsync;
        _seedDatabase = factory.SeedDataBaseAsync;
    }
    
    protected async Task<TResult> ExecuteHandler<THandler, TResult>(
        Func<THandler, Task<TResult>> action)
        where THandler : notnull
    {
        await using var scope = _services.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<THandler>();
        return await action(handler);
    }
    
    protected async Task<T> ExecuteInDb<T>(Func<DirectoryServiceDbContext, Task<T>> action)
    {
        await using var initScope = _services.CreateAsyncScope();
        var context = initScope.ServiceProvider.GetRequiredService<DirectoryServiceDbContext>();

        return await action(context);
    }
    
    protected async Task ExecuteInDb(Func<DirectoryServiceDbContext, Task> action)
    {
        await using var initScope = _services.CreateAsyncScope();
        var context = initScope.ServiceProvider.GetRequiredService<DirectoryServiceDbContext>();

        await action(context);
    }

    public Task InitializeAsync() => _seedDatabase();

    public Task DisposeAsync() => _resetDatabase();
    
}