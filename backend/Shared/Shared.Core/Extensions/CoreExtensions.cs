using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Abstractions;

namespace Shared.Core.Extensions;

public static class CoreExtensions
{
    public static IServiceCollection AddCoreHandlers(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => 
                classes.AssignableToAny(
                    typeof(ICommandHandler<,>),
                    typeof(ICommandHandler<>),
                    typeof(IQueryHandler<,>),
                    typeof(IQueryHandler<>)))
            .AsSelfWithInterfaces().WithScopedLifetime());

        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}