using CSharpFunctionalExtensions;
using DirectoryService.Domain.ValueObjects;
using General;
using General.Errors;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Domain.Departments;

public class Department
{
    public Guid Id { get; private set; }
    public DepartmentName DepartmentName { get; private set; } = null!;
    public Identifier Identifier { get; private set; } = null!;
   
    public Guid? ParentId { get; private set; }
    public Department? Parent { get; private set; }
    public Path Path { get; private set; } = null!;
    public short Depth { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<Department> _childrenDepartments = [];
    private readonly List<DepartmentLocation> _departmentLocations = [];
    private readonly List<DepartmentPosition> _departmentPositions = [];
    
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;
    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;
    public IReadOnlyList<Department> ChildrenDepartments => _childrenDepartments;
    
    public bool HasLocation => _departmentLocations.Count > 0;
   
    // EF Core v
    private Department()
    {
    }
    
    private Department(
        Guid id, 
        DepartmentName departmentName,
        Identifier identifier,
        Path path,
        short depth,
        bool isActive,
        IEnumerable<DepartmentLocation> departmentLocations,
        DateTime createdAt,
        DateTime updatedAt,
        Department? parent = null)
    {
        Id = id;
        DepartmentName = departmentName;
        Identifier = identifier;
        Path = path;
        Depth = depth;
        IsActive = isActive;
        _departmentLocations = departmentLocations.ToList();
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Parent = parent;
    }
    
    public static Result<Department, FailList> CreateParent(
        DepartmentName departmentName,
        Identifier identifier,
        IEnumerable<DepartmentLocation> departmentLocations,
        Guid? id = null)
    {
        var path = ValueObjects.Path.CreateParent(identifier);
        var now = DateTime.UtcNow;
        return new Department(id ?? Guid.NewGuid(), departmentName, identifier, path, 0, true, departmentLocations, now, now);
    }
    
    public static Result<Department, FailList> CreateChild(
        DepartmentName departmentName,
        Identifier identifier,
        Department parent,
        IEnumerable<DepartmentLocation> departmentLocations,
        Guid? id = null)
    {
        var path = parent.Path.CreateChild(identifier);
        var now = DateTime.UtcNow;
        short depth = (short)(parent.Depth + 1);
        return new Department(id ?? Guid.NewGuid(), departmentName, identifier, path, depth, true, departmentLocations, now, now, parent);
    }

    public void SetParent(Guid? parentId) => ParentId = parentId;

    public void SetPath(Path path) => Path = path;
    public void SetDepth(short depth) => Depth = depth;
    
    public void UpdateLocations(IEnumerable<Guid> locationIds)
    {
        var list = locationIds.Select(id => new DepartmentLocation(Id, id));
        _departmentLocations.Clear();
        _departmentLocations.AddRange(list);
    }
    
    /// <summary>
    /// Обновляет коллекцию локаций департамента.
    /// </summary>
    /// <param name="departmentLocations">Новый список локаций.</param>
    /// <remarks>
    /// При передаче сущностей с заполненным Id EF Core пометит их как <c>Modified</c>
    /// и сгенерирует UPDATE вместо INSERT. Если соответствующих записей в БД не существует —
    /// выбрасывается <see cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException"/>.
    /// Используй конструктор без Id либо добавляй записи напрямую через DbSet.AddRangeAsync.
    /// </remarks>
    public void UpdateLocations(IEnumerable<DepartmentLocation> departmentLocations)
    {
        _departmentLocations.Clear();
        _departmentLocations.AddRange(departmentLocations);
    }
    
 }
