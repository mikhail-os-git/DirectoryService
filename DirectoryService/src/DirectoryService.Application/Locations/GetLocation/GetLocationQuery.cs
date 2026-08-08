using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Locations.GetLocation;

public record GetLocationQuery(Guid LocationId): IQuery;