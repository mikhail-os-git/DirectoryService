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
}