namespace DirectoryService.Contracts.Departments.CreateDepartment;

public record CreateDepartmentRequest(string Name, string Identifier, IReadOnlyList<Guid> LocationIds, Guid? ParentId = null);