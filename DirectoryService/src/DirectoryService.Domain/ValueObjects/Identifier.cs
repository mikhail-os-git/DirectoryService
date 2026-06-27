using System.Globalization;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Common.Constants;
using General;
using General.Errors;

namespace DirectoryService.Domain.ValueObjects;

public record Identifier
{
    public const int MAX_LENGTH = LengthConstants.MAX_LENGTH_150;
    public const int MIN_LENGTH = LengthConstants.MIN_LENGTH_2;
    private const char SEPARATOR = '-';
    public string Value { get; }

    private Identifier(string value)
    {
        Value = value.ToLower(CultureInfo.InvariantCulture);
    }

    public static Result<Identifier, Failure> Create(string value)
    {
        if (StringValidator.IsEmpty(value))
            return Failure.Validation("The value must not be empty.", "identifier.is.invalid");
        
        if (!StringValidator.Required(value, MAX_LENGTH, MIN_LENGTH))
        {
            string message = $"The number of characters in the value is too large or too small. The value size should be from {MAX_LENGTH} to {MIN_LENGTH}";
            return Failure.Validation(message, "identifier.is.invalid");
        }
        
        if (value.StartsWith(SEPARATOR) || value.EndsWith(SEPARATOR))
            return Failure.Validation($"Identifier cannot start or end with '{SEPARATOR}'", "identifier.is.invalid");
        
        if (value.Contains($"{SEPARATOR}{SEPARATOR}", StringComparison.InvariantCulture))
            return Failure.Validation($"Identifier cannot contain consecutive '{SEPARATOR}' separators", "identifier.is.invalid");

        bool isValid = value.Contains(SEPARATOR, StringComparison.InvariantCulture)
            ? StringValidator.IsEnglishWordWithSeparator(value, SEPARATOR)
            : StringValidator.IsEnglishWord(value);

        if (!isValid)
            return Failure.Validation($"Identifier must contain only English letters or the '{SEPARATOR}' separator", "identifier.is.invalid");
        
        return new Identifier(value.ToLowerInvariant());

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