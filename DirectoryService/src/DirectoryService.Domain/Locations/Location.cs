using CSharpFunctionalExtensions;
using DirectoryService.Domain.Relations;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain.Locations;

public class Location
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; }
    public Address Address { get; private set; }
    public Timezone Timezone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private List<DepartmentLocation> _departmentLocations = [];
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;
    
    private Location(Guid id, Name name, Address address, Timezone timezone, bool isActive, DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Address = address;
        Timezone = timezone;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Result<Location, string> Create(Name name, Address address, Timezone timezone)
    {
        Guid id = Guid.NewGuid();
        var now = DateTime.UtcNow;

        return new Location(id, name, address, timezone, true, now, now);
    }

    public void AddDepartment(Guid departmentId)
    {
        _departmentLocations.Add(new DepartmentLocation(departmentId, Id));
    }
}