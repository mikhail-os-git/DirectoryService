using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Extensions;

namespace DirectoryService.Application.Configuration;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection UseCaseRegistration(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationDependencyInjection).Assembly;
        services.AddCoreHandlers(assembly);
        return services;
    }
}