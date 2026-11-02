namespace DirectoryService.Infrastructure.Database;

public interface IDataSeeder
{
    Task SeedAsync();
}