using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Common.Constants;
using General;
using General.Errors;

namespace DirectoryService.Domain.ValueObjects;

public record Identifier
{
    public const int MAX_LENGTH = LengthConstants.MAX_LENGTH_150;
    public const int MIN_LENGTH = LengthConstants.MIN_LENGTH_3;
    public string Value { get; }

    private Identifier(string value)
    {
        Value = value;
    }

    public static Result<Identifier, Failure> Create(string value)
    {
        if (StringValidator.IsEmpty(value))
        {
            return Failure.Validation("The value must not be empty.", "identifier.is.invalid");
        }
        else if (!StringValidator.Required(value, MAX_LENGTH, MIN_LENGTH))
        {
            string message = $"The number of characters in the value is too large or too small. The value size should be from {MAX_LENGTH} to {MIN_LENGTH}";
            return Failure.Validation(message, "identifier.is.invalid");
        }
        
        if (!StringValidator.IsEnglishWord(value))
            return Failure.Validation("The identifier must contain only English letters.");
        
        return new Identifier(value);

    }
    
    /// <summary>
    /// Создаёт экземпляр из сырой строки без доменной валидации.
    /// Только для десериализации в инфраструктурном слое (EF Core и т.п.).
    /// Для создания из пользовательского ввода используй <see cref="Create"/>.
    /// </summary>
    /// <param name="value">Сырая строка, прочитанная из источника данных.</param>
    /// <returns>Экземпляр <see cref="Identifier"/>.</returns>
    public static Identifier Convert(string value) => new(value);
}