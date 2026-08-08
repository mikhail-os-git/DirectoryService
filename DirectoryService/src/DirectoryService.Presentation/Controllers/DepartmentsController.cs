using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Departments.CreateDepartment;
using DirectoryService.Application.Departments.GetDepartment;
using DirectoryService.Application.Departments.MoveDepartment;
using DirectoryService.Application.Departments.UpdateDepartmentLocations;
using DirectoryService.Contracts.Departments;
using DirectoryService.Contracts.Departments.CreateDepartment;
using DirectoryService.Contracts.Departments.GetDepartment;
using DirectoryService.Contracts.Departments.MoveDepartment;
using DirectoryService.Contracts.Departments.UpdateDepartment;
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
        [FromServices] ICommandHandler<Guid, CreateDepartmentCommand> handler, 
        CancellationToken cancellationToken = default)
    {
        return await handler.Handle(new CreateDepartmentCommand(request), cancellationToken);
    }

    [HttpPut("{departmentId:guid}/locations")]
    public async Task<EndpointResult<Guid>> UpdateLocations(
        [FromRoute] Guid departmentId,
        [FromBody] UpdateDepartmentLocationsRequest request,
        [FromServices] ICommandHandler<Guid, UpdateDepartmentLocationsCommand> handler,
        CancellationToken cancellationToken) => await handler.Handle(new UpdateDepartmentLocationsCommand(departmentId, request.LocationIds), cancellationToken);

    [HttpPut("{departmentId:guid}/parent")]
    public async Task<EndpointResult<Guid>> MoveDepartment(
        [FromRoute] Guid departmentId,
        [FromBody] MoveDepartmentRequest request,
        [FromServices] ICommandHandler<Guid, MoveDepartmentCommand> handler,
        CancellationToken cancellationToken) => await handler.Handle(new MoveDepartmentCommand(departmentId, request.parentId), cancellationToken);
    
    [HttpGet("{id:guid}")]
    public async Task<EndpointResult<GetDepartmentResponse>> GetDepartment(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetDepartmentResponse, GetDepartmentQuery> handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new GetDepartmentQuery(id), cancellationToken);
}