using DirectoryService.Contracts.Positions.CreatePosition;
using Shared.Core.Abstractions;

namespace DirectoryService.Application.Positions.CreatePosition;

public record CreatePositionCommand(CreatePositionRequest Request) : ICommand;