namespace DirectoryService.Contracts.Locations.GetLocation;

public record GetLocationResponse
{
    public Guid Id { get; init; }
    public string LocationName { get; init; } = null!;
    public string Address { get; init; } = null!;
    public string Timezone { get; init; } = null!;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}