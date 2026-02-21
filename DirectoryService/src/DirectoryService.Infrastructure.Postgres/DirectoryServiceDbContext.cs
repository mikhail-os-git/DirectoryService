using DirectoryService.Infrastructure.Departments;
using DirectoryService.Infrastructure.Locations;
using DirectoryService.Infrastructure.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DirectoryService.Infrastructure;

public class DirectoryServiceDbContext : DbContext
{
    public DirectoryServiceDbContext(DbContextOptions<DirectoryServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DirectoryServiceDbContext).Assembly);
    }

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Location> Locations => Set<Location>();

    public DbSet<Position> Positions => Set<Position>();
    
}
