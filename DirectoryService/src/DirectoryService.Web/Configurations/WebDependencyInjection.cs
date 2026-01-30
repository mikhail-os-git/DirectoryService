using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.Configurations;
using Microsoft.OpenApi;
using Serilog;
using Serilog.Exceptions;

namespace DirectoryService.Web.Configurations;

public static class WebDependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
       return services
           .AddLogger(configuration)
           .AddInfrastructurePostgres(configuration);

    }
    
    public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ServiceName", "DirectoryService"));
        
        return services;
    }
}