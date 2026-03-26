using System.Text.Json;
using System.Text.Json.Serialization;

namespace General.Errors;

public record Failure
{
    public string Code { get; }
    public string Message { get; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FailureType Type { get; }
    public string? InvalidField { get; }

    private Failure(string code, string message, FailureType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }

    public static Failure Validation(string message, string? code = null, string? invalidField = null) =>
        new(code ?? "value.is.invalid", message, FailureType.VALIDATION, invalidField);
    
    public static Failure NotFound(string message, Guid? id, string? code = null) =>
        new(code ?? "record.not.found", message, FailureType.NOT_FOUND);

    public static Failure Conflict(string message, string? code = null) => new(code ?? "value.conflict", message, FailureType.CONFLICT);

    public static Failure Error(string message, string? code = null) =>
        new(code ?? "error", message, FailureType.ERROR);

    public static Failure None => new Failure(string.Empty, string.Empty, FailureType.NONE, null);

    public FailList ToFailList() => this;

    public override string ToString() => JsonSerializer.Serialize(this);
}

public enum FailureType
{
    /// <summary>
    /// Пустая ошибка
    /// </summary>
    NONE,

    /// <summary>
    /// Ошибка с валидацией
    /// </summary>
    VALIDATION,
    
    /// <summary>
    /// Ошибка, ничего не найдено
    /// </summary>
    NOT_FOUND,
    
    /// <summary>
    /// Ошибка серверка
    /// </summary>
    ERROR,
    
    /// <summary>
    /// Ошибка конфликт
    /// </summary>
    CONFLICT
}