using General.Errors;
using Microsoft.AspNetCore.Http;

namespace General.EndpointsResult;

public sealed class FailureResult: IResult
{
    private FailList _errors;
    public FailureResult(Failure failure)
    {
        _errors = failure.ToFailList();
    }

    public FailureResult(FailList failures)
    {
        _errors = failures;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        if (_errors.Count == 0)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return httpContext.Response.WriteAsJsonAsync(Envelope.Error(_errors));
        }

        var types = _errors
            .Select(f => f.Type)
            .Distinct()
            .ToList();

        int status = types.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCodeFromType(_errors.First().Type);

        var envelope = Envelope.Error(_errors);
        httpContext.Response.StatusCode = status;
        return httpContext.Response.WriteAsJsonAsync(envelope);
    }

    private static int GetStatusCodeFromType(FailureType type)
    {
        return type switch
        {
            FailureType.VALIDATION => StatusCodes.Status400BadRequest,
            FailureType.NOT_FOUND => StatusCodes.Status404NotFound,
            FailureType.CONFLICT => StatusCodes.Status409Conflict,
            FailureType.ERROR => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}