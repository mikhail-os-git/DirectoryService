namespace DirectoryService.Contracts.Departments.GetAllDepartments;

public class GetAllDepartmentsItem
{
    public Guid Id { get; init; }
    public string DepartmentName { get; init; } = null!;
    public string Path { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}