using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Configuration;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Application.Positions.Interfaces;
using DirectoryService.Application.Tasks;
using DirectoryService.Domain.Common.Constants;
using DirectoryService.Infrastructure.Database;
using DirectoryService.Infrastructure.Options;
using DirectoryService.Infrastructure.Repositories;
using DirectoryService.Infrastructure.Tasks;
using DirectoryService.Infrastructure.Workers;
using Microsoft.AspNetCore.Builder;
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
        return services.UseCaseRegistration()
            .DbContextRegistration()
            .AddRepositories()
            .AddTransactionTools()
            .ConfigureInfrastructureOptions(configuration)
            .AddInfrastructureTasks()
            .AddInfrastructureWorkers();
    }
    
    private static IServiceCollection DbContextRegistration(this IServiceCollection services)
    {
        services.AddDbContextPool<DirectoryServiceDbContext>((sp, options) =>
        {
            IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
            string? connectionString = configuration.GetConnectionString(DatabaseConstants.DATABASE);
            IHostEnvironment hostEnv = sp.GetRequiredService<IHostEnvironment>();
            ILoggerFactory loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            options.UseNpgsql(connectionString);

            if (hostEnv.IsDevelopment() || hostEnv.IsEnvironment("Testing"))
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }

            options.UseLoggerFactory(loggerFactory);
        });
        
        services.AddScoped<IDirectoryReadDbContext>(provider => 
            provider.GetRequiredService<DirectoryServiceDbContext>());
        
        return services;
    }

    private static IServiceCollection AddTransactionTools(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, NpgSqlConnectionFactory>();
        services.AddScoped<ITransactionManager, TransactionManager>();
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Здесь будет регестрация всех Репозиториев
        services.AddScoped<ILocationsRepository, LocationsRepository>();
        services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
        services.AddScoped<IPositionsRepository, PositionsRepository>();
        return services;
    }

    private static IServiceCollection ConfigureInfrastructureOptions(this IServiceCollection services, IConfiguration configurations)
    {
        services.AddOptions<DeleteWorkerOptions>()
            .Bind(configurations.GetSection($"{ApplicationConstants.WORKER_SETTINGS}:{DeleteWorkerOptions.NAME}"))
            .Validate(
                o => o.BatchSize > 0 
                     && o.Interval > TimeSpan.Zero 
                     && o.Tables.Values.Any(t => t.RetentionPeriod > TimeSpan.Zero),
                "Invalid DeleteWorker config")
            .ValidateOnStart();
        return services;
    }

    private static IServiceCollection AddInfrastructureTasks(this IServiceCollection services)
    {
        services.AddScoped<ICleanupTask, CleanupDepartmentsTask>();
        services.AddScoped<ICleanupTask, CleanupLocationsTask>();
        services.AddScoped<ICleanupTask, CleanupPositionsTask>();
        return services;
    }

    private static IServiceCollection AddInfrastructureWorkers(this IServiceCollection services)
    {
        services.AddHostedService<DeleteBackgroundService>();
        return services;
    }
    
}