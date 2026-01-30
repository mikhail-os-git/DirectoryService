using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Common.Constants;

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

    public static Result<Identifier, string> Create(string value)
    {
        if (StringValidator.IsEmpty(value))
        {
            return "The value must not be empty.";
        }
        else if (!StringValidator.Required(value, MAX_LENGTH, MIN_LENGTH))
        {
            return $"The number of characters in the value is too large or too small. The value size should be from {MAX_LENGTH} to {MIN_LENGTH}";

        }
        
        if (!StringValidator.IsEnglishWord(value))
            return "The identifier must contain only English letters.";
        
        return new Identifier(value);

    }
    
    public static Identifier FromDb(string value) => new(value);
}