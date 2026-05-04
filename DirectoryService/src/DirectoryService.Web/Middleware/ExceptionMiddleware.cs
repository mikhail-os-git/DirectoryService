using DirectoryService.Domain.Common;
using General;
using General.Errors;

namespace DirectoryService.Web.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception with message: {Message}", exception.Message);

            var envelope = Envelope.Error(CommonErrors.InternalError);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}

public static class ExceptionMiddlewareRegister
{
    public static IApplicationBuilder UseExceptionMiddleware(this WebApplication app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}