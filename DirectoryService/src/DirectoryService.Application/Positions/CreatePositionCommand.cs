using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Positions;

namespace DirectoryService.Application.Positions;

public record CreatePositionCommand(CreatePositionRequest Request) : ICommand;