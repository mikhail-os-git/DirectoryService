using Microsoft.AspNetCore.Builder;

namespace Shared.Framework.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseAppMiddlewares(this WebApplication application)
    {
       return application.UseMiddleware<ExceptionMiddleware>().UseMiddleware<RequestCorrelationIdMiddleware>();
    }
}