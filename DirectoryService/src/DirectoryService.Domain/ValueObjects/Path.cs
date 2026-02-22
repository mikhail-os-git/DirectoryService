using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;

namespace DirectoryService.Domain.ValueObjects;

public record Path
{
    public string Value { get; }

    private readonly List<string> _roads = [];

    public IReadOnlyList<string> Roads => _roads;

    private Path(string value)
    {
        Value = value;
        _roads = SplitRoads(value);
    }

    public static Result<Path, string> Create(string value)
    {
        if (StringValidator.IsEmpty(value))
        {
            return "The value must not be empty.";
        }
        
        foreach (char ch in value)
        {
            if(ch == '.' || ch == '-') continue;
            if (!StringValidator.IsEnglishLetter(ch))
            {
                return $"The path can contain only English letters and symbols: '.' and '-";
            }
        }
        
        return new Path(value);
    }

    public static Path FromDb(string path)
    {
        return new Path(path);
    }
    
    private static List<string> SplitRoads(string path)
    {
        return path.Split('.').ToList();
    }
}