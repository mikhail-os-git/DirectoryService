using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Positions;
using DirectoryService.Application.Positions.CreatePosition;
using DirectoryService.Application.Positions.HardDeletePosition;
using DirectoryService.Application.Positions.RenamePosition;
using DirectoryService.Application.Positions.SoftDeletePosition;
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

    [HttpDelete("{id:guid}")]
    public async Task<EndpointResult<Guid>> HardDeletePosition(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<Guid, SoftDeletePositionCommand> handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new SoftDeletePositionCommand(id), cancellationToken);

    [HttpPatch("{id:guid}")]
    public async Task<EndpointResult<Guid>> RenamePosition(
        [FromRoute] Guid id,
        [FromBody] RenamePositionRequest request,
        [FromServices] ICommandHandler<Guid, RenamePositionCommand> handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(
            new RenamePositionCommand(id, request.NewName),
            cancellationToken);
}