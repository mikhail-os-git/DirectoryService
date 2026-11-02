using Shared.Core.Abstractions;

namespace DirectoryService.Application.Positions.RenamePosition;

public record RenamePositionCommand(Guid PositionId, string NewName) : ICommand;