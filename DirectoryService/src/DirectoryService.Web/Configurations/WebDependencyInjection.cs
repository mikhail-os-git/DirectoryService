using DirectoryService.Infrastructure.Configurations;
using General.Converters;
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
    
    public static WebApplication Configure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        // if (app.Environment.IsDevelopment())
        // {
        // }
        app.MapOpenApi();
    
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "DirectoryService"));  
        
        return app;
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
        services.AddControllers().AddJsonOptions(opt =>
        {
            // Регистрация Guid Converter
            opt.JsonSerializerOptions.Converters.Add(new GuidJsonConverter());
        });
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