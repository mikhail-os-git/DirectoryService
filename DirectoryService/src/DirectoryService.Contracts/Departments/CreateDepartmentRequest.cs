namespace DirectoryService.Contracts.Departments;

public record CreateDepartmentRequest(string Name, string Identifier, IReadOnlyList<Guid> LocationIds, Guid? ParentId = null);