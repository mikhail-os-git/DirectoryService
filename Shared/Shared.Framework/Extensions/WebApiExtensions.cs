using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Shared.Framework.Converters;

namespace Shared.Framework.Extensions;

public static class WebApiExtensions
{
    public static IServiceCollection AddSerilogLogging(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        services.AddSerilog((sp, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(sp)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ServiceName", serviceName));

        return services;
    }

    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(opt =>
            opt.JsonSerializerOptions.Converters.Add(new GuidJsonConverter()));

        services.AddOpenApi();

        services.Configure<ApiBehaviorOptions>(opt =>
            opt.SuppressModelStateInvalidFilter = true);

        return services;
    }

    public static WebApplication UseWebApi(this WebApplication app, string serviceName)
    {
        app.UseSerilogRequestLogging();
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
            options.SwaggerEndpoint("/openapi/v1.json", serviceName));

        return app;
    }
}