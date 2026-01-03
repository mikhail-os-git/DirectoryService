using CSharpFunctionalExtensions;
using DirectoryService.Domain.Relations;
using DirectoryService.Domain.ValueObjects;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Domain.Departments;

public class Department
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Identifier Identifier { get; private set; }
    public Guid? ParentId { get; private set; }
    public Path Path { get; private set; }
    public short Depth { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private List<DepartmentLocation> _departmentLocations = [];
    private List<DepartmentPosition> _departmentPositions = [];
    
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;
    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;
    
    private Department(
        Guid id, 
        Name name,
        Identifier identifier,
        Guid? parentId,
        Path path,
        short depth,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    
    public static Result<Department, string> Create(Name name, Identifier identifier, Path path, short depth, Guid? parentId = null)
    {
        Guid id = Guid.NewGuid();
        DateTime now = DateTime.UtcNow;
        
        return new Department(id, name, identifier, parentId, path, depth, true, now, now);
        
    }

    public void AddLocation(Guid locationId)
    {
        _departmentLocations.Add(new DepartmentLocation(Id, locationId));
    }

    public void AddPosition(Guid positionId)
    {
        _departmentPositions.Add(new DepartmentPosition(Id, positionId));
    }
    
}
