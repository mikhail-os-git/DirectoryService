using CSharpFunctionalExtensions;
using DirectoryService.Infrastructure.Common;
using DirectoryService.Infrastructure.Common.Constants;

namespace DirectoryService.Infrastructure.ValueObjects;

public record PositionName
{
    public const int MAX_LENGTH = LengthConstants.MAX_LENGTH_100;
    public const int MIN_LENGTH = LengthConstants.MIN_LENGTH_3;
    
    public string Value { get; }

    private PositionName(string value)
    {
        Value = value;
    }

    public static Result<PositionName, string> Create(string value)
    {
        if (StringValidator.IsEmpty(value))
        {
            return "The value must not be empty.";
        }
        else if (!StringValidator.Required(value, MAX_LENGTH, MIN_LENGTH))
        {
            return $"The number of characters in the value is too large or too small. The value size should be from {MAX_LENGTH} to {MIN_LENGTH}";
        }

        return new PositionName(value);
    }

    public static PositionName FromDb(string name)
    {
        return new PositionName(name);
    }
}