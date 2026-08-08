using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Positions;
using DirectoryService.Application.Positions.CreatePosition;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Positions;
using DirectoryService.Contracts.Positions.CreatePosition;
using General.EndpointsResult;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PositionsController: ControllerBase
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromBody] CreatePositionRequest request,
        [FromServices] ICommandHandler<Guid, CreatePositionCommand> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(new CreatePositionCommand(request), cancellationToken);
    }
}