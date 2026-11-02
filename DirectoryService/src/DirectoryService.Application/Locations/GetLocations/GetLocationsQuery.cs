using DirectoryService.Contracts.Locations.GetLocations;
using Shared.Core.Abstractions;

namespace DirectoryService.Application.Locations.GetLocations;

public record GetLocationsQuery(GetLocationsRequest Request): IQuery;