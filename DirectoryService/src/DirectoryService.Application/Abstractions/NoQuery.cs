namespace DirectoryService.Application.Abstractions;

public sealed record NoQuery : IQuery
{
    public static readonly NoQuery Value = new();
}