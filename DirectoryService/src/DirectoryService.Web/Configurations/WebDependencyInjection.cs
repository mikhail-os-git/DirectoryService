using DirectoryService.Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Exceptions;

namespace DirectoryService.Web.Configurations;

public static class WebDependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddLogger(configuration)
            .AddInfrastructurePostgres(configuration)
            .AddControllersAndOpenApi()
            .ConfigureApiBehaviorOptions();

    }
    
    private static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((sp, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(sp)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ServiceName", "DirectoryService"));
        
        return services;
    }

    private static IServiceCollection AddControllersAndOpenApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
    
    private static IServiceCollection ConfigureApiBehaviorOptions(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(opt =>
        {
            opt.SuppressModelStateInvalidFilter = true;
        });

        return services;
    }
}