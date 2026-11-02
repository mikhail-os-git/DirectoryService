using System.Text.Json.Serialization;
using Shared.Kernel.Failures;

namespace Shared.Kernel.Envelopes;

public record Envelope
{
    public object? Result { get; }
    public FailList? Errors { get; }
    public DateTime TimeGenerated { get; }

    public bool IsError => Errors is { Count: > 0 };
    
    [JsonConstructor]
    private Envelope(object? result, FailList? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) => new(result, null);
    public static Envelope Error(FailList errors) => new(null, errors);
}

public record Envelope<T>
{
    public T? Result { get; }
    public FailList? Errors { get; }
    public DateTime TimeGenerated { get; }

    public bool IsError => Errors is { Count: > 0 };
    
    [JsonConstructor]
    private Envelope(T? result, FailList? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope<T> Ok(T? result = default) => new(result, null);
    public static Envelope<T> Error(FailList errors) => new(default, errors);
}