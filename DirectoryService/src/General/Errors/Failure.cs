using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace General.Errors;

public record Failure
{
    public string Code { get; }
    public string Message { get; }
    
    public Guid? EntityId { get; }
    
    public FailureType Type { get; }
    public string? InvalidField { get; }
    
    private const char MAIN_SEPARATOR = '|';
    private const char SECOND_SEPARATOR = ':';
    private Failure(string code, string message, FailureType type, string? invalidField = null, Guid? entityId = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
        EntityId = entityId;
    }

    public static Failure Validation(string message, string? code = null, string? invalidField = null) =>
        new(code ?? "value.is.invalid", message, FailureType.VALIDATION, invalidField);
    
    public static Failure NotFoundEntity(string message, Guid? id, string? code = null) =>
        new(code ?? "record.not.found", message, FailureType.NOT_FOUND, null, id);
    
    public static Failure NotFoundCollectionEntity(string message, string? code = null) =>
        new(code ?? "records.not.found", message, FailureType.NOT_FOUND);
    
    public static Failure Conflict(string message, string? code = null) => new(code ?? "value.conflict", message, FailureType.CONFLICT);

    public static Failure ConflictEntity(string message, string? code = null, Guid? entityId = null) => new(code ?? "value.conflict", message, FailureType.CONFLICT, null, entityId);
    
    public static Failure Error(string message, string? code = null) =>
        new(code ?? "error", message, FailureType.ERROR);

    public static Failure Authentication(string message, string? code = null) =>
        new(code ?? "authentication.failure", message, FailureType.AUTHENTICATION);
    
    public static Failure Authorization(string message, string? code = null) =>
        new(code ?? "authorization.failure", message, FailureType.AUTHORIZATION);
    public static Failure None => new Failure(string.Empty, string.Empty, FailureType.NONE, null);

    public FailList ToFailList() => this;

    public override string ToString() => JsonSerializer.Serialize(this);

    public static Failure Deserialize(string failureString)
    {
        string[] rows = failureString.Split(MAIN_SEPARATOR);
        if (rows.Length < 4)
            throw new FormatException($"Invalid failure string format: '{failureString}'");

        Dictionary<string, string> failDictionary = new();
        foreach (string row in rows)
        {
            string[] splited = row.Split(SECOND_SEPARATOR, 2);
            string key = splited[0].Trim();
            if (key != nameof(Code) &&
                key != nameof(Message) &&
                key != nameof(Type) &&
                key != nameof(InvalidField) &&
                key != nameof(EntityId))
            {
                throw new FormatException($"Unknown field '{key}' in failure string: '{failureString}'");
            }

            failDictionary.Add(key, splited[1].Trim());
        }

        if(!Enum.TryParse<FailureType>(failDictionary[nameof(Type)], out FailureType type))
            throw new ArgumentException($"Unknown FailureType value: '{failDictionary[nameof(Type)]}'", paramName: nameof(failureString));

        var fail = new Failure(
            failDictionary[nameof(Code)], 
            failDictionary[nameof(Message)],
            type,
            failDictionary[nameof(InvalidField)] == "null" ? null : failDictionary[nameof(InvalidField)],
            failDictionary[nameof(EntityId)] == "null" ? null : Guid.Parse(failDictionary[nameof(EntityId)]));
        return fail;
    }
    
    public static string Serialize(Failure failure)
    {
        string res = $@"{nameof(Code)} {SECOND_SEPARATOR} {failure.Code} {MAIN_SEPARATOR} 
                        {nameof(Message)} {SECOND_SEPARATOR} {failure.Message} {MAIN_SEPARATOR} 
                        {nameof(Type)} {SECOND_SEPARATOR} {failure.Type} {MAIN_SEPARATOR}
                        {nameof(InvalidField)} {SECOND_SEPARATOR} {failure.InvalidField ?? "null"} {MAIN_SEPARATOR}
                        {nameof(EntityId)} {SECOND_SEPARATOR} {(failure.EntityId.HasValue ? failure.EntityId.ToString() : "null")}";

        return res;
    }

    public string Serialize()
    {
        return Serialize(this);
    }
}

[JsonConverter(typeof(JsonStringEnumConverter<FailureType>))]
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
    CONFLICT,
    
    /// <summary>
    /// Ошибка аутентификации
    /// </summary>
    AUTHENTICATION,
    
    /// <summary>
    /// Ошибка авторизации
    /// </summary>
    AUTHORIZATION
}