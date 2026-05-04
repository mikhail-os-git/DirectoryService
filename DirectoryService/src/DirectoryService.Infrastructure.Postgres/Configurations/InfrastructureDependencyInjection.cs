using DirectoryService.Application.Configuration;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Application.Positions.Interfaces;
using DirectoryService.Domain.Common.Constants;
using DirectoryService.Infrastructure.Database;
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
}