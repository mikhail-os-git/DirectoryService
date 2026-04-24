namespace DirectoryService.Contracts.Positions;

public record CreatePositionRequest(string Name, IReadOnlyList<Guid> DepartmentIds, string? Description = null);