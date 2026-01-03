using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects;

public record Identifier
{
    public const int MAX_LENGTH = 150;
    public string Value { get; }

    private Identifier(string value)
    {
        Value = value;
    }

    public static Result<Identifier, string> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "The value must not be empty.";
        }
        else if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
        {
            return "the number of characters in the value is too small.";
        }
        else if (value.Length >= MAX_LENGTH)
        {
            return "the number of characters in the value is too large.";
        }
        
        if (!value.All(char.IsAsciiLetter))
            return "The identifier must contain only English letters.";
        
        return new Identifier(value);
    }
}