using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Locations.GetLocations;

namespace DirectoryService.Application.Locations.GetLocations;

public record GetLocationsQuery(GetLocationsRequest Request): IQuery;