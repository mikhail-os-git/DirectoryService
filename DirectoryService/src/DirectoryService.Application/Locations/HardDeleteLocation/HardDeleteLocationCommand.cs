using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Locations.HardDeleteLocation;

public record HardDeleteLocationCommand(Guid LocationId): ICommand;