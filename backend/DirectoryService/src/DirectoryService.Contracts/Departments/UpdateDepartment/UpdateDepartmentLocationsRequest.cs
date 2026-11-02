namespace DirectoryService.Contracts.Departments.UpdateDepartment;

public record UpdateDepartmentLocationsRequest(IReadOnlyList<Guid> LocationIds);