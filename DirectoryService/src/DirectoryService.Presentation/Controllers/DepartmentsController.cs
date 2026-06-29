using DirectoryService.Application.Departments;
using DirectoryService.Contracts.Departments;
using General.EndpointsResult;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromBody] CreateDepartmentRequest request,
        [FromServices] CreateDepartmentHandler handler, 
        CancellationToken cancellationToken = default)
    {
        return await handler.Handle(new CreateDepartmentCommand(request), cancellationToken);
    }

    [HttpPut("{departmentId:guid}/locations")]
    public async Task<EndpointResult<Guid>> UpdateLocations(
        [FromRoute] Guid departmentId,
        [FromBody] UpdateDepartmentLocationsRequest request,
        [FromServices] UpdateDepartmentLocationsHandler handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(new UpdateDepartmentLocationsCommand(departmentId, request.LocationIds), cancellationToken);
    }

    [HttpPut("{departmentId:guid}/parent")]
    public async Task<EndpointResult<Guid>> MoveDepartment(
        [FromRoute] Guid departmentId,
        [FromBody] MoveDepartmentRequest request,
        [FromServices] MoveDepartmentHandler handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(new MoveDepartmentCommand(departmentId, request.parentId), cancellationToken);
    }
}