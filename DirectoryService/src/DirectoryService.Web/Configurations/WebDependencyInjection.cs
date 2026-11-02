using DirectoryService.Infrastructure.Configurations;
using DirectoryService.Infrastructure.Database;
using DirectoryService.TestData;
using Shared.Framework.Extensions;

namespace DirectoryService.Web.Configurations;

public static class WebDependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSerilogLogging(configuration, "DirectoryService")
            .AddWebApi()
            .AddInfrastructurePostgres(configuration)
            .AddSeeder(); 

    }
    
    public static WebApplication Configure(this WebApplication app)
    {
        app.UseWebApi("DirectoryService");
        return app;
    }

    private static IServiceCollection AddSeeder(this IServiceCollection services)
    { 
#if  DEBUG
        services.AddScoped<IDataSeeder, RandomDataSeeder>();  
#endif
        return services;
    }

    public static async Task<WebApplication> RunSeedAsync(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return app;
#if DEBUG
        await using var scope = app.Services.CreateAsyncScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync();
        }
#endif
        return app;
    }
}