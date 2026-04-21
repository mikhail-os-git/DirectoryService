using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Locations.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application.Configuration;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection UseCaseRegistration(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationDependencyInjection).Assembly;
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => 
                classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces().WithScopedLifetime());

        services.AddValidatorsFromAssembly(typeof(ApplicationDependencyInjection).Assembly);
        return services;
    }
}