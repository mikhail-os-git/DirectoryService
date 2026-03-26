using System.Reflection;
using CSharpFunctionalExtensions;
using General.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace General.EndpointsResult;

public sealed class EndpointResult<TValue> : IResult, IEndpointMetadataProvider
{
    private readonly IResult _result;

    public EndpointResult(Result<TValue, FailList> result)
    {
        _result = result.IsSuccess
            ? new SuccessResult<TValue>(result.Value)
            : new FailureResult(result.Error);
    }
    
    public EndpointResult(Result<TValue, Failure> result)
    {
        _result = result.IsSuccess
            ? new SuccessResult<TValue>(result.Value)
            : new FailureResult(result.Error);
    }

    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(builder);

        builder.Metadata.Add(new ProducesResponseTypeMetadata(200, typeof(Envelope<TValue>), ["application/json"]));
        builder.Metadata.Add(new ProducesResponseTypeMetadata(500, typeof(Envelope<TValue>), ["application/json"]));
        builder.Metadata.Add(new ProducesResponseTypeMetadata(400, typeof(Envelope<TValue>), ["application/json"]));
        builder.Metadata.Add(new ProducesResponseTypeMetadata(404, typeof(Envelope<TValue>), ["application/json"]));
        builder.Metadata.Add(new ProducesResponseTypeMetadata(401, typeof(Envelope<TValue>), ["application/json"]));
        builder.Metadata.Add(new ProducesResponseTypeMetadata(403, typeof(Envelope<TValue>), ["application/json"]));
        builder.Metadata.Add(new ProducesResponseTypeMetadata(409, typeof(Envelope<TValue>), ["application/json"])); 
    }

    public static implicit operator EndpointResult<TValue>(Result<TValue, FailList> result) => new EndpointResult<TValue>(result);

    public static implicit operator EndpointResult<TValue>(Result<TValue, Failure> result) => new EndpointResult<TValue>(result);
    
    public Task ExecuteAsync(HttpContext httpContext) => _result.ExecuteAsync(httpContext);
}
