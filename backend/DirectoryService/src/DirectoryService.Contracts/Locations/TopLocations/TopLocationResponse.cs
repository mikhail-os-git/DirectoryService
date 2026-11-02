namespace DirectoryService.Contracts.Locations.TopLocations;

public record TopLocationResponse(IReadOnlyList<TopLocationItem> Items);

public record TopLocationItem
{
    public Guid Id { get; init; }
    public string LocationName { get; init; } = null!;
    public string Address { get; init; } = null!;
    public int DepartmentCount { get; init; }

}