using CSharpFunctionalExtensions;
using DirectoryService.Domain.Relations;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain.Positions;

public class Position
{
    public const int MAX_DESCRIPTION_LENGTH = 1000;
    public Guid Id { get; private set; }
    public Name Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private List<DepartmentPosition> _departmentPositions = [];
    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;
    
    private Position(Guid id, Name name, string? description, bool isActive, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Result<Position, string> Create(Name name, bool isActive,
        string? description = null)
    {
        Guid id = Guid.NewGuid();
        var now = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(description))
        {
            if (description.Length > MAX_DESCRIPTION_LENGTH)
            {
                return $"the description text is too long, the maximum number of characters: {MAX_DESCRIPTION_LENGTH}";
            }
        }

        return new Position(id, name, description, isActive, now, now);
    }
    
    public void AddDepartment(Guid departmentId)
    {
        _departmentPositions.Add(new DepartmentPosition(departmentId, Id));
    }
}