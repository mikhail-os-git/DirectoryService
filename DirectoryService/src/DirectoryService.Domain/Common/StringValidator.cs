namespace DirectoryService.Domain.Common;

public static class StringValidator
{
    public static bool IsEnglishLetter(char letter)
    {
        return char.IsAsciiLetter(letter);
    }

    public static bool IsEnglishWord(string word)
    {
        return word.All(char.IsAsciiLetter);
    }

    public static bool IsEmpty(string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static bool Required(string value, int max, int min = 0)
    {
        return value.Length >= min && value.Length <= max;
    }
}