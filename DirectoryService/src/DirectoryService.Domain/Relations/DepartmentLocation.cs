namespace DirectoryService.Domain.Relations;

public sealed class DepartmentLocation
{
    public Guid DepartmentId { get; private set; }
    public Guid LocationId { get; private set; }

    public DepartmentLocation(Guid departmentId, Guid locationId)
    {
        DepartmentId = departmentId;
        LocationId = locationId;
    }
}