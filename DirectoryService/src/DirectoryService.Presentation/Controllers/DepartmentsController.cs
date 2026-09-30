using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Departments.AttachPosition;
using DirectoryService.Application.Departments.CreateDepartment;
using DirectoryService.Application.Departments.DetachPosition;
using DirectoryService.Application.Departments.GetAllDepartments;
using DirectoryService.Application.Departments.GetDepartment;
using DirectoryService.Application.Departments.HardDeleteDepartment;
using DirectoryService.Application.Departments.MoveDepartment;
using DirectoryService.Application.Departments.UpdateDepartmentLocations;
using DirectoryService.Contracts.Common;
using DirectoryService.Contracts.Departments;
using DirectoryService.Contracts.Departments.AttachPosition;
using DirectoryService.Contracts.Departments.CreateDepartment;
using DirectoryService.Contracts.Departments.DetachPosition;
using DirectoryService.Contracts.Departments.GetAllDepartments;
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

    [HttpGet("all")]
    public async Task<EndpointResult<PagedResult<GetAllDepartmentsItem>>> GetAll(
        [FromQuery] GetAllDepartmentsRequest request,
        [FromServices] IQueryHandler<PagedResult<GetAllDepartmentsItem>, GetAllDepartmentsQuery> handler,
        CancellationToken cancellationToken)
    {
        request = request with
        {
            PageSettings = request.PageSettings is null ? new PageSettings() : request.PageSettings
        };

        return await handler.Handle(new GetAllDepartmentsQuery(request), cancellationToken);
    }

    [HttpDelete("{id:guid}")]
    public async Task<EndpointResult<Guid>> HardDeleteDepartment(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<Guid, HardDeleteDepartmentCommand> handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new HardDeleteDepartmentCommand(id), cancellationToken);

    [HttpPost("{deptId:guid}/positions/{posId:guid}")]
    public async Task<EndpointResult<AttachPositionResponse>> AttachPosition(
        [FromRoute] Guid deptId,
        [FromRoute] Guid posId,
        [FromServices] ICommandHandler<AttachPositionResponse, AttachPositionCommand> handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new AttachPositionCommand(deptId, posId), cancellationToken);
    
    [HttpDelete("{deptId:guid}/positions/{posId:guid}")]
    public async Task<EndpointResult<DetachPositionResponse>> DetachPosition(
        [FromRoute] Guid deptId,
        [FromRoute] Guid posId,
        [FromServices] ICommandHandler<DetachPositionResponse, DetachPositionCommand> handler,
        CancellationToken cancellationToken) =>
        await handler.Handle(new DetachPositionCommand(deptId, posId), cancellationToken);
    
}