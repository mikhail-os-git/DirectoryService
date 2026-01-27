using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;

namespace DirectoryService.Domain.ValueObjects;

public record Timezone
{
    public string Value { get; }

    private Timezone(string value)
    {
        Value = value;
    }

    public static Result<Timezone, string> Create(string value)
    {
        if (StringValidator.IsEmpty(value))
        {
            return "The timezone must be specified.";
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

        return "The time zone was specified incorrectly.";
    }
}