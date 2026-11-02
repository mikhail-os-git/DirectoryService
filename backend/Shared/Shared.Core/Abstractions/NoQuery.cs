namespace Shared.Core.Abstractions;

public sealed record NoQuery : IQuery
{
    public static readonly NoQuery Value = new();
}