using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Positions.HardDeletePosition;

public record HardDeletePositionCommand(Guid PositionId) : ICommand;