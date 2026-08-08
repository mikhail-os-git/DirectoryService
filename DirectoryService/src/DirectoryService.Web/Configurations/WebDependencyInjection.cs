using DirectoryService.Infrastructure.Configurations;
using DirectoryService.Infrastructure.Database;
using DirectoryService.TestData;
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
            .ConfigureApiBehaviorOptions()
            .AddSeeder();

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

    private static IServiceCollection AddSeeder(this IServiceCollection services)
    { 
#if  DEBUG
        services.AddScoped<IDataSeeder, DataSeeder>();  
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