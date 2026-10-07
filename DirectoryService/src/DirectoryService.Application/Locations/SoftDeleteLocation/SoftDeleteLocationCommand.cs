using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Locations.SoftDeleteLocation;

public record SoftDeleteLocationCommand(Guid LocationId): ICommand;