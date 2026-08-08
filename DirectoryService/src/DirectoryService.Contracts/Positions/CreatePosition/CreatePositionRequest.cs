namespace DirectoryService.Contracts.Positions.CreatePosition;

public record CreatePositionRequest(string Name, IReadOnlyList<Guid> DepartmentIds, string? Description = null);