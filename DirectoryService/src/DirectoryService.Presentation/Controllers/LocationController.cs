using DirectoryService.Infrastructure.Locations;
using DirectoryService.Infrastructure.Locations.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LocationController: ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] LocationRequestDto request,
        [FromServices] ILocationCreateHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(result.Value);
    }
}