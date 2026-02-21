using DirectoryService.Infrastructure.Locations;
using DirectoryService.Infrastructure.Locations.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Infrastructure.Configuration;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection UseCaseRegistration(this IServiceCollection services)
    {
        // Здесть будет регестрация всех хендлеров
        services.AddScoped<ILocationCreateHandler, LocationCreateHandler>();
        return services;
    }
}