using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects;

public record Path
{
    public string Value { get; }

    private List<string> _roads = [];

    public IReadOnlyList<string> Roads => _roads;

    private Path(string value)
    {
        Value = value;
        _roads = value.Split('.').ToList();
    }

    private static Result<Path, string> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "The value must not be empty.";
        }
        
        foreach (char ch in value)
        {
            if(ch == '.' || ch == '-') continue;
            if (!char.IsAsciiLetter(ch))
            {
                return $"The path can contain only English letters and symbols: '.' and '-";
            }
        }
        
        return new Path(value);
    }
}