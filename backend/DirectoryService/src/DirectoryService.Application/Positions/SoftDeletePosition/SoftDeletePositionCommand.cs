using Shared.Core.Abstractions;

namespace DirectoryService.Application.Positions.SoftDeletePosition;

public record SoftDeletePositionCommand(Guid PositionId): ICommand;