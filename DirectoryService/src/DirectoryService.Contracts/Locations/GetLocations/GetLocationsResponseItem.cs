namespace DirectoryService.Contracts.Locations.GetLocations;

public record GetLocationsResponseItem
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Address { get; set; } = null!;
    public DateTime CreatedAt { get; init; }
}