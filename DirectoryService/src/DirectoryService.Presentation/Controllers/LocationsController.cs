using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Locations.CreateLocation;
using DirectoryService.Application.Locations.GetLocation;
using DirectoryService.Application.Locations.GetLocations;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Contracts.Common;
using DirectoryService.Contracts.Locations;
using DirectoryService.Contracts.Locations.CreateLocation;
using DirectoryService.Contracts.Locations.GetLocation;
using DirectoryService.Contracts.Locations.GetLocations;
using DirectoryService.Contracts.Locations.TopLocations;
using General.EndpointsResult;
using General.Errors;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromBody] CreateLocationRequest request,
        [FromServices] ICommandHandler<Guid, CreateLocationCommand> handler,
        CancellationToken cancellationToken = default) =>
        await handler.Handle(new CreateLocationCommand(request), cancellationToken);

    [HttpGet("{id:guid}")]
    public async Task<EndpointResult<GetLocationResponse>> GetLocation(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetLocationResponse, GetLocationQuery> handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new GetLocationQuery(id), cancellationToken);

    [HttpGet("top")]
    public async Task<EndpointResult<TopLocationResponse>> TopLocations(
        [FromServices] IQueryHandler<TopLocationResponse, IQuery> handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(NoQuery.Value, cancellationToken);
    
    [HttpGet]
    public async Task<EndpointResult<PagedResult<GetLocationsResponseItem>>> GetLocations(
        [FromQuery] GetLocationsRequest request,
        [FromServices] IQueryHandler<PagedResult<GetLocationsResponseItem>, GetLocationsQuery> handler,
        CancellationToken cancellationToken)
    {
        request = request with
        {
            PageSettings = request.PageSettings is null ? new PageSettings() : request.PageSettings
        };
        return await handler.Handle(new GetLocationsQuery(request), cancellationToken);
    }
}