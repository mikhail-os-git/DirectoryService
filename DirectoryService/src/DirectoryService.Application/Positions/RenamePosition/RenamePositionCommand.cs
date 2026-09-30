using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Positions.RenamePosition;

public record RenamePositionCommand(Guid PositionId, string NewName, bool IncludeInactive) : ICommand;