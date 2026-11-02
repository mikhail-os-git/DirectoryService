using CSharpFunctionalExtensions;
using Shared.Kernel.Failures;

namespace Shared.Kernel.Envelopes;

public static class EnvelopeExtensions
{
    public static Result<T, FailList> ToResult<T>(this Envelope<T> envelope)
    {
        if(envelope.Errors is null && envelope.Result is null)
            return CommonErrors.InternalError.ToFailList();
        
        if (envelope.IsError)
            return envelope.Errors!;

        if (envelope.Result is null)
            return CommonErrors.InternalError.ToFailList();

        return envelope.Result;
    }
    
    public static UnitResult<FailList> ToUnitResult(this Envelope envelope)
    {
        if(envelope.Errors is null && envelope.Result is null)
            return CommonErrors.InternalError.ToFailList();
        
        if (envelope.IsError)
            return envelope.Errors!;

        return UnitResult.Success<FailList>();
    }
}