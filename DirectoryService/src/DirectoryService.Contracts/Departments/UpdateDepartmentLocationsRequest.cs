namespace DirectoryService.Contracts.Departments;

public record UpdateDepartmentLocationsRequest(IReadOnlyList<Guid> LocationIds);