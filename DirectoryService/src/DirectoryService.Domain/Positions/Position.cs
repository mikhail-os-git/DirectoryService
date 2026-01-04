using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Relations;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain.Positions;

public class Position
{
    public Guid Id { get; private set; }
    public PositionName PositionName { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private readonly List<DepartmentPosition> _departmentPositions = [];
    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;
    
    private Position(Guid id, PositionName positionName, string? description, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        PositionName = positionName;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Result<Position, string> Create(PositionName positionName, bool isActive,
        string? description = null)
    {
        Guid id = Guid.NewGuid();
        var now = DateTime.UtcNow;

        if (!StringValidator.IsEmpty(description))
        {
            int max = 1000;
            if (!StringValidator.Required(description!, max))
            {
                return $"the description text is too long, the maximum number of characters: {max}";
            }
        }

        return new Position(id, positionName, description, isActive, now, now);
    }
    
    public void AddDepartments(params Guid[] departmentIds)
    {
        foreach (var id in departmentIds)
        { 
            if(id != Guid.Empty && !CheckDepartment(id))
                _departmentPositions.Add(new DepartmentPosition(id, Id));
        }
    }
    
    private bool CheckDepartment(Guid id)
    {
        return _departmentPositions.Any(dp => dp.DepartmentId == id);
    }
}