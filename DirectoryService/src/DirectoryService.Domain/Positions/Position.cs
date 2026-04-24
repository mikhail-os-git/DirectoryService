using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Common.Constants;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.ValueObjects;
using General;
using General.Errors;

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

    // EF Core
    private Position()
    {
    }
    
    private Position(Guid id, PositionName positionName, IEnumerable<DepartmentPosition> departmentPositions, string? description, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        PositionName = positionName;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        _departmentPositions = departmentPositions.ToList();
    }

    public static Result<Position, Failure> Create(
        PositionName positionName,
        IEnumerable<DepartmentPosition> departmentPositions,
        string? description = null, Guid? id = null)
    {
        var now = DateTime.UtcNow;

        if (!StringValidator.IsEmpty(description))
        {
            if (!StringValidator.Required(description!, LengthConstants.MAX_LENGTH_1000))
            {
                string message =
                    $"the description text is too long, the maximum number of characters: {LengthConstants.MAX_LENGTH_1000}";
                return Failure.Validation(message, "position.description.is.invalid");
            }
        }

        return new Position(id ?? Guid.NewGuid(), positionName, departmentPositions, description, true, now, now);
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