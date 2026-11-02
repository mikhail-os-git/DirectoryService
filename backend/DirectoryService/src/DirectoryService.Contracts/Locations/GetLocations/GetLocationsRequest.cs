using DirectoryService.Contracts.Common;

namespace DirectoryService.Contracts.Locations.GetLocations;

public record GetLocationsRequest
{
    public string Search { get; init; } = null!;
    public string SortBy { get; init; } = "name";
    public string SortDir { get; init; } = "asc";
    public int DepartmentCount { get; init; }
    public PageSettings PageSettings { get; init; } = null!;
}