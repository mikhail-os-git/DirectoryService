using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using General;
using General.Errors;

namespace DirectoryService.Domain.ValueObjects;

public record Timezone
{
    public string Value { get; }

    private Timezone(string value)
    {
        Value = value;
    }

    public static Result<Timezone, Failure> Create(string value)
    {
        if (StringValidator.IsEmpty(value))
        {
            return Failure.Validation("The timezone must be specified.", "timezone.is.invalid");
        }
        
        value = value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal);
        string regex = @"^[A-Za-z0-9_+\-/]+$";
        if (
            value.Contains('/', StringComparison.Ordinal)
            && !value.StartsWith('/')
            && !value.EndsWith('/')
            && !value.Contains("//", StringComparison.Ordinal)
            && Regex.IsMatch(value, regex)
        )
        {
            return new Timezone(value);
        }
        
        return Failure.Validation("The time zone was specified incorrectly.", "timezone.is.invalid");

    }

    /// <summary>
    /// Создаёт экземпляр из сырой строки без доменной валидации.
    /// Только для десериализации в инфраструктурном слое (EF Core и т.п.).
    /// Для создания из пользовательского ввода используй <see cref="Create"/>.
    /// </summary>
    /// <param name="value">Сырая строка, прочитанная из источника данных.</param>
    /// <returns>Экземпляр <see cref="Timezone"/>.</returns>
    public static Timezone Convert(string value) => new(value);

    public static implicit operator string(Timezone timezone) => timezone.Value;
}