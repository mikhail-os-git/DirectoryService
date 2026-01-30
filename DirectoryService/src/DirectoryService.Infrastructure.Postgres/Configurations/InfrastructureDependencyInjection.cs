using DirectoryService.Domain.Common.Constants;
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
        // Repositories здесь создать и зарегестрировать
        services.AddDbContextPool<DirectoryServiceDbContext>((sp, options) =>
        {
            string? connectionString = configuration.GetConnectionString(DatabaseConstants.DATABASE);
            IHostEnvironment hostEnviroment = sp.GetRequiredService<IHostEnvironment>();
            ILoggerFactory loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            options.UseNpgsql(connectionString);

            if (hostEnviroment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }

            options.UseLoggerFactory(loggerFactory);
        });

        return services;
    }
}