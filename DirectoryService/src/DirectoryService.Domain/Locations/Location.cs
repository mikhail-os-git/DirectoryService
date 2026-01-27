using CSharpFunctionalExtensions;
using DirectoryService.Domain.Relations;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain.Locations;

public class Location
{
    public Guid Id { get; private set; }
    public LocationName LocationName { get; private set; }
    public Address Address { get; private set; }
    public Timezone Timezone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private readonly List<DepartmentLocation> _departmentLocations = [];
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;
    
    private Location(Guid id, LocationName locationName, Address address, Timezone timezone, bool isActive, DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        LocationName = locationName;
        Address = address;
        Timezone = timezone;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Result<Location, string> Create(LocationName locationName, Address address, Timezone timezone)
    {
        Guid id = Guid.NewGuid();
        var now = DateTime.UtcNow;

        return new Location(id, locationName, address, timezone, true, now, now);
    }

    public void AddDepartments(params Guid[] departmentIds)
    {
        foreach (Guid id in departmentIds)
        {
            if(id != Guid.Empty && !CheckDepartment(id))
                _departmentLocations.Add(new DepartmentLocation(id, Id));
        }
    }

    private bool CheckDepartment(Guid id)
    {
        return _departmentLocations.Any(dl => dl.DepartmentId == id);
    }
}