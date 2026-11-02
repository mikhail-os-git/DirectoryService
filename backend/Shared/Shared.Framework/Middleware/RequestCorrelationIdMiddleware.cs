using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace Shared.Framework.Middleware;

public class RequestCorrelationIdMiddleware
{
    private const string CORRELATION_ID_HEADER_NAME = "X-Correlation-Id";
    private const string CORRELATION_ID = "CorrelationId";
    private readonly RequestDelegate _next;

    public RequestCorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Headers.TryGetValue(CORRELATION_ID_HEADER_NAME, out StringValues correlationValues);

        string corrId = correlationValues.FirstOrDefault() ?? context.TraceIdentifier;

        using (LogContext.PushProperty(CORRELATION_ID, corrId))
        {
            await _next(context);
        }
    }
}

public static class CorrelationIdMiddlewareRegister
{
    public static IApplicationBuilder UseCorrelationMiddleware(this WebApplication app)
    {
        return app.UseMiddleware<RequestCorrelationIdMiddleware>();
    }
}