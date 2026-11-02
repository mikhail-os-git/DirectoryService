namespace DirectoryService.Contracts.Departments.GetDapartmentAncestors;

public record GetDepartmentAncestorsItem
{
    public Guid Id { get; init; }
    public Guid ParentId { get; init; }
    public string DepartmentName { get; init; } = null!;
    public string Identifier { get; init; } = null!;
    public string Path { get; init; } = null!;
    public short Depth { get; init; }
}