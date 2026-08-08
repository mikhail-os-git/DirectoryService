using DirectoryService.Application.Database;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Positions;
using DirectoryService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DirectoryService.Infrastructure;

public class DirectoryServiceDbContext : DbContext, IDirectoryReadDbContext
{
    public DirectoryServiceDbContext(DbContextOptions<DirectoryServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("ltree");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DirectoryServiceDbContext).Assembly);
    }

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Location> Locations => Set<Location>();

    public DbSet<Position> Positions => Set<Position>();

    public DbSet<DepartmentLocation> DepartmentLocations => Set<DepartmentLocation>();

    public IQueryable<Department> DepartmentsQuery => Departments.AsNoTracking().AsQueryable();
    public IQueryable<Location> LocationsQuery => Locations.AsNoTracking().AsQueryable();
    public IQueryable<Position> PositionsQuery => Positions.AsNoTracking().AsQueryable();
    public IQueryable<DepartmentLocation> DepartmentLocationsQuery => DepartmentLocations.AsNoTracking().AsQueryable();
}
