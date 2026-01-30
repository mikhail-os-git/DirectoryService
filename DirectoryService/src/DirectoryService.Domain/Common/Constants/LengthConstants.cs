using System.Diagnostics.CodeAnalysis;

namespace DirectoryService.Domain.Common.Constants;

#pragma warning disable CA1815
public readonly struct LengthConstants
{
    public const int MAX_LENGTH_150 = 150;
    public const int MAX_LENGTH_120 = 120;
    public const int MAX_LENGTH_100 = 100;
    public const int MAX_LENGTH_1000 = 1000;
    public const int MIN_LENGTH_3 = 3;
}
#pragma warning restore CA1815