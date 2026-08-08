namespace DirectoryService.Contracts.Departments.GetDepartment;

public record GetDepartmentResponse
{
    public Guid Id { get; init; }
    public string DepartmentName { get; init; } = null!;
    public string Identifier { get; init; } = null!;
    public Guid? ParentId { get; init; }
    public string Path { get; init; } = null!;
    public short Depth { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}