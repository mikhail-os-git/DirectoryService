using System.Net;
using Microsoft.AspNetCore.Http;

namespace General.EndpointsResult;

public sealed class SuccessResult<TValue> : IResult
{
    private readonly TValue _value;

    public SuccessResult(TValue value)
    {
        _value = value;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        var envelope = Envelope.Ok(_value);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

        await httpContext.Response.WriteAsJsonAsync(envelope);

    }

}