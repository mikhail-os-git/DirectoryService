using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Positions;

namespace DirectoryService.Application.Database;

public interface IDirectoryReadDbContext
{
    public IQueryable<Department> DepartmentsQuery { get; }

    public IQueryable<Location> LocationsQuery { get; }

    public IQueryable<Position> PositionsQuery { get; }

    public IQueryable<DepartmentLocation> DepartmentLocationsQuery { get; }
}