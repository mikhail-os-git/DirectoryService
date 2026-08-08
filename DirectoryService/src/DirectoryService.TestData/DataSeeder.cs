using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.Database;
using DirectoryService.TestData.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.TestData;

public class DataSeeder: IDataSeeder
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(DirectoryServiceDbContext dbContext, ILogger<DataSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            if (await _dbContext.Locations.AnyAsync()) return;

            await _dbContext.Locations.AddRangeAsync(LocationFixtures.All);
            await _dbContext.Departments.AddRangeAsync(DepartmentFixtures.All);
            await _dbContext.Positions.AddRangeAsync(PositionFixtures.All);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            
            _logger.LogInformation("Seeding completed.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Seeding failed.");
            await transaction.RollbackAsync();
        }
        
    }

}