using Shared.Core.Abstractions;

namespace DirectoryService.Application.Locations.HardDeleteLocation;

public record HardDeleteLocationCommand(Guid LocationId): ICommand;