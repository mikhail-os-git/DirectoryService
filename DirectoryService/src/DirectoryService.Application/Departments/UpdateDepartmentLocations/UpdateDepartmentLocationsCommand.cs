using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Departments.UpdateDepartmentLocations;

public record UpdateDepartmentLocationsCommand(Guid DepartmentId, IReadOnlyList<Guid> LocationIds) : ICommand;