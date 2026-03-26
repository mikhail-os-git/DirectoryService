using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Common.Constants;
using General;
using General.Errors;

namespace DirectoryService.Domain.ValueObjects;

public record PositionName
{
    public const int MAX_LENGTH = LengthConstants.MAX_LENGTH_100;
    public const int MIN_LENGTH = LengthConstants.MIN_LENGTH_3;
    
    public string Value { get; }

    private PositionName(string value)
    {
        Value = value;
    }

    public static Result<PositionName, Failure> Create(string value)
    {
        if (StringValidator.IsEmpty(value))
        {
            return Failure.Validation("The value must not be empty.", "position-name.is.invalid");
        }
        else if (!StringValidator.Required(value, MAX_LENGTH, MIN_LENGTH))
        {
            string message =
                $"The number of characters in the value is too large or too small. The value size should be from {MAX_LENGTH} to {MIN_LENGTH}";
            return Failure.Validation(message, "position-name.is.invalid");
        }

        return new PositionName(value);
    }

    public static PositionName FromDb(string name)
    {
        return new PositionName(name);
    }
}