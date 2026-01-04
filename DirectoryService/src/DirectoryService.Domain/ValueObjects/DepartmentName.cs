using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;

namespace DirectoryService.Domain.ValueObjects;

public record DepartmentName
{
    public const int MAX_LENGTH = 150;
    public const int MIN_LENGTH = 3;
    
    public string Value { get; }

    private DepartmentName(string value)
    {
        Value = value;
    }

    public static Result<DepartmentName, string> Create(string value)
    {
        if (StringValidator.IsEmpty(value))
        {
            return "The value must not be empty.";
        }
        else if (!StringValidator.Required(value, MAX_LENGTH, MIN_LENGTH))
        {
            return $"The number of characters in the value is too large or too small. The value size should be from {MAX_LENGTH} to {MIN_LENGTH}";
        }
        
        return new DepartmentName(value);
    }
}