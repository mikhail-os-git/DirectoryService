using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects;

public record Name
{
    public const int MAX_LENGTH = 150;
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name, string> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "The value must not be empty.";
        }
        else if (value.Length < 3)
        {
            return "the number of characters in the value is too small";
        }
        else if (value.Length >= MAX_LENGTH)
        {
            return "the number of characters in the value is too large";
        }
        
        return new Name(value);
    }
}