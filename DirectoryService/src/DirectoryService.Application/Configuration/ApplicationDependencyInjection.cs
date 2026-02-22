using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Locations.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application.Configuration;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection UseCaseRegistration(this IServiceCollection services)
    {
        // Здесть будет регестрация всех хендлеров
        var assembly = typeof(ApplicationDependencyInjection).Assembly;
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => 
                classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces().WithScopedLifetime());
        return services;
    }
}