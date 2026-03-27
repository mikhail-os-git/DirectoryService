using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using General;
using General.Errors;

namespace DirectoryService.Domain.ValueObjects;

public record Timezone
{
    public string Value { get; }

    private Timezone(string value)
    {
        Value = value;
    }

    public static Result<Timezone, Failure> Create(string value)
    {
        if (StringValidator.IsEmpty(value))
        {
            return Failure.Validation("The timezone must be specified.", "timezone.is.invalid");
        }
        
        value = value.Trim().Replace(" ", string.Empty, StringComparison.Ordinal);
        string regex = @"^[A-Za-z0-9_+\-/]+$";
        if (
            value.Contains('/', StringComparison.Ordinal)
            && !value.StartsWith('/')
            && !value.EndsWith('/')
            && !value.Contains("//", StringComparison.Ordinal)
            && Regex.IsMatch(value, regex)
        )
        {
            return new Timezone(value);
        }
        
        return Failure.Validation("The time zone was specified incorrectly.", "timezone.is.invalid");

    }

    public static Timezone FromDb(string timezone)
    {
        return new Timezone(timezone);
    }
}