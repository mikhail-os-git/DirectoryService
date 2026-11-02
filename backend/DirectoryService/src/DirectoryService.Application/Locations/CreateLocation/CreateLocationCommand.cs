using DirectoryService.Contracts.Locations;
using DirectoryService.Contracts.Locations.CreateLocation;
using Shared.Core.Abstractions;

namespace DirectoryService.Application.Locations.CreateLocation;

public record CreateLocationCommand(CreateLocationRequest Request) : ICommand;