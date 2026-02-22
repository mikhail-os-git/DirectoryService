namespace DirectoryService.Domain.Departments;

public sealed class DepartmentPosition
{
    public Guid Id { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid PositionId { get; private set; }

    public DepartmentPosition(Guid departmentId, Guid positionId)
    {
        Id = Guid.NewGuid();
        DepartmentId = departmentId;
        PositionId = positionId;
    }
    
    // EF Core
    private DepartmentPosition()
    {
    }
}