namespace DirectoryService.Domain.Departments;

public sealed class DepartmentPosition
{
    public Guid Id { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid PositionId { get; private set; }

    public DepartmentPosition(Guid id, Guid departmentId, Guid positionId)
    {
        Id = id;
        DepartmentId = departmentId;
        PositionId = positionId;
    }
    
    public DepartmentPosition(Guid departmentId, Guid positionId)
    {
        Id = Guid.Empty;
        DepartmentId = departmentId;
        PositionId = positionId;
    }
    
    // EF Core
    private DepartmentPosition()
    {
    }
}