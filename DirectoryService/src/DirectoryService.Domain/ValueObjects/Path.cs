using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using General;
using General.Errors;

namespace DirectoryService.Domain.ValueObjects;

public record Path
{
    private const char Separetor = '/';
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
        if (StringValidator.IsEmpty(value))
        {
            return Failure.Validation("The value must not be empty.", "path.is.invalid");
        }
        
        foreach (char ch in value)
        {
            if(ch == Separetor) continue;
            if (!StringValidator.IsEnglishLetter(ch))
            {
                string message = $"The path can contain only English letters and symbols: '.' and '-";
                return Failure.Validation(message, "path.is.invalid");
            }
        }
        
        return new Path(value);
    }

    public static Path CreateParent(Identifier identifier)
    {
        return new(identifier.Value);
    }

    public Path CreateChild(Identifier identifier)
    {
        return new Path(Value + Separetor + identifier.Value);
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
        return path.Split(Separetor).ToList();
    }
}