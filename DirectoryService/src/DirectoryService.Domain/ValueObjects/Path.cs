using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using General;
using General.Errors;

namespace DirectoryService.Domain.ValueObjects;

public record Path
{
    private const char IDENTIFIER_SEPARATOR = '-';
    private const char PATH_SEPARATOR = '.';
    
    public string Value { get; }

    private readonly List<string> _roads = [];

    public IReadOnlyList<string> Roads => _roads;

    private Path(string value)
    {
        Value = value;
        _roads = SplitRoads(value);
    }

    public static Result<Path, Failure> CreateFromString(string value)
    {
        return Validate(value).Map(() => new Path(value));
    }

    public static Path CreateParent(Identifier identifier)
    {
        return new(identifier.Value);
    }

    public Path CreateChild(Identifier identifier)
    {
        return new Path(Value + PATH_SEPARATOR + identifier.Value);
    }

    /// <summary>
    /// Создаёт экземпляр из сырой строки без доменной валидации.
    /// Только для десериализации в инфраструктурном слое (EF Core и т.п.).
    /// Для создания из пользовательского ввода используй <see cref="Create"/>.
    /// </summary>
    /// <param name="value">Сырая строка, прочитанная из источника данных.</param>
    /// <returns>Экземпляр <see cref="Path"/>.</returns>
    public static Path Convert(string value) => new(value);
    
    private static List<string> SplitRoads(string path)
    {
        return path.Split(PATH_SEPARATOR).ToList();
    }

    private static UnitResult<Failure> Validate(string value)
    {
        if (StringValidator.IsEmpty(value))
        {
            return UnitResult.Failure(Failure.Validation("The value must not be empty.", "path.is.invalid"));
        }
        
        foreach (char ch in value)
        {
            if(ch == PATH_SEPARATOR || ch == IDENTIFIER_SEPARATOR) continue;
            if (!StringValidator.IsEnglishLetter(ch))
            {
                string message = $"The path can contain only English letters and symbols: '{PATH_SEPARATOR}' and '{IDENTIFIER_SEPARATOR}";
                return UnitResult.Failure(Failure.Validation(message, "path.is.invalid"));
            }
        }

        return UnitResult.Success<Failure>();
    }

    public static implicit operator string(Path path) => path.Value;

}