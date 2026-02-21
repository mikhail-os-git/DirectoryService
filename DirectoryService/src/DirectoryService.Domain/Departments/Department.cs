using CSharpFunctionalExtensions;
using DirectoryService.Infrastructure.ValueObjects;
using Path = DirectoryService.Infrastructure.ValueObjects.Path;

namespace DirectoryService.Infrastructure.Departments;

public class Department
{
    public Guid Id { get; private set; }
    public DepartmentName DepartmentName { get; private set; } = null!;
    public Identifier Identifier { get; private set; } = null!;
    public Guid? ParentId { get; private set; }
    public Path Path { get; private set; } = null!;
    public short Depth { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<DepartmentLocation> _departmentLocations = [];
    private readonly List<DepartmentPosition> _departmentPositions = [];
    
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;
    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;
    
    // EF Core
    private Department()
    {
    }
    
    private Department(
        Guid id, 
        DepartmentName departmentName,
        Identifier identifier,
        Guid? parentId,
        Path path,
        short depth,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        DepartmentName = departmentName;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    
    public static Result<Department, string> Create(DepartmentName departmentName, Identifier identifier, Path path, short depth, Guid? parentId = null)
    {
        Guid id = Guid.NewGuid();
        DateTime now = DateTime.UtcNow;
        
        return new Department(id, departmentName, identifier, parentId, path, depth, true, now, now);
        
    }

    public Department AddLocations(params Guid[] locationIds)
    {
        foreach (Guid id in locationIds)
        {
            if(id != Guid.Empty && !CheckLocation(id))
                _departmentLocations.Add(new DepartmentLocation(Id, id));
        }

        return this;
    }
    
    public Department AddPositions(params Guid[] positionIds)
    {
        foreach (Guid id in positionIds)
        {
            if(id != Guid.Empty && !CheckPosition(id))
                _departmentPositions.Add(new DepartmentPosition(Id, id));
        }

        return this;
    }
    
    private bool CheckLocation(Guid id)
    {
        return _departmentLocations.Any(dl => dl.LocationId == id);
    }

    private bool CheckPosition(Guid id)
    {
        return _departmentPositions.Any(dp => dp.PositionId == id);
    }
}
