namespace DirectoryService.Infrastructure.Departments;

public sealed class DepartmentLocation
{
    public Guid Id { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid LocationId { get; private set; }

    public DepartmentLocation(Guid departmentId, Guid locationId)
    {
        Id = Guid.NewGuid();
        DepartmentId = departmentId;
        LocationId = locationId;
    }

    // EF Core
    private DepartmentLocation()
    {
    }
    
}