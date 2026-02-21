using DirectoryService.Infrastructure.Common.Constants;
using DirectoryService.Infrastructure.Configuration;
using DirectoryService.Infrastructure.Locations.Interfaces;
using DirectoryService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Configurations;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructurePostgres(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.UseCaseRegistration().DbContextRegistration(configuration).AddRepositories();
    }
    
    private static IServiceCollection DbContextRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<DirectoryServiceDbContext>((sp, options) =>
        {
            string? connectionString = configuration.GetConnectionString(DatabaseConstants.DATABASE);
            IHostEnvironment hostEnv = sp.GetRequiredService<IHostEnvironment>();
            ILoggerFactory loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            options.UseNpgsql(connectionString);

            if (hostEnv.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }

            options.UseLoggerFactory(loggerFactory);
        });

        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Здесь будет регестрация всех Репозиториев
        return services.AddScoped<ILocationsRepository, LocationsRepository>();
    }
}