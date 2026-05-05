namespace DirectoryService.Domain.Departments;

public sealed class DepartmentLocation
{
    public Guid Id { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid LocationId { get; private set; }

    public DepartmentLocation(Guid id, Guid departmentId, Guid locationId)
    {
        Id = id;
        DepartmentId = departmentId;
        LocationId = locationId;
    }
    
    public DepartmentLocation(Guid departmentId, Guid locationId)
    {
        Id = Guid.Empty;
        DepartmentId = departmentId;
        LocationId = locationId;
    }

    // EF Core
    private DepartmentLocation()
    {
    }
    
}