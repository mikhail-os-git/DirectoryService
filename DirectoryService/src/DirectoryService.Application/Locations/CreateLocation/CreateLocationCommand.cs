using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Locations;
using DirectoryService.Contracts.Locations.CreateLocation;

namespace DirectoryService.Application.Locations.CreateLocation;

public record CreateLocationCommand(CreateLocationRequest Request) : ICommand;