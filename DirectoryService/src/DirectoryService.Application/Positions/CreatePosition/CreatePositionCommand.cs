using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Positions;
using DirectoryService.Contracts.Positions.CreatePosition;

namespace DirectoryService.Application.Positions.CreatePosition;

public record CreatePositionCommand(CreatePositionRequest Request) : ICommand;