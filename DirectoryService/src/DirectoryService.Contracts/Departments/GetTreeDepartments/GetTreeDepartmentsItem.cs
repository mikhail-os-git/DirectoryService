namespace DirectoryService.Contracts.Departments.GetDepartmentsTree;

public record GetTreeDepartmentsItem
{
    public Guid Id { get; init; }
    public string DepartmentName { get; init; } = null!;
    public string Identifier { get; init; } = null!;
    public string Path { get; init; } = null!;
    public short Depth { get; init; }
    public bool HasChildren { get; init; }
}