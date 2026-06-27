using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.Database;
using DirectoryService.TestData.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.TestData;

public class DataSeeder: IDataSeeder
{
    private readonly DirectoryServiceDbContext _dbContext;

    public DataSeeder(DirectoryServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
        if (await _dbContext.Locations.AnyAsync()) return;

        await _dbContext.Locations.AddRangeAsync(LocationFixtures.All);
        await _dbContext.Departments.AddRangeAsync(DepartmentFixtures.All);
        await _dbContext.Positions.AddRangeAsync(PositionFixtures.All);
    
        await _dbContext.SaveChangesAsync();
    }

}