using System.Diagnostics.CodeAnalysis;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.ValueObjects;
using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.TestData;

/// <summary>
/// Генерирует ~10 000 записей в сумме:
///   1 000 локаций   (~15% без департаментов)
///   5 000 департаментов (50 корней, глубина до 6)
///   4 000 позиций   (~10% без департаментов, часть — в 2–3 департаментах)
/// Seed фиксированный → при каждом запуске одни и те же данные.
/// </summary>
[SuppressMessage("Security", "CA5394:Не используйте небезопасные генераторы случайных чисел.", Justification = "<Pending>")]
[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1404:Code analysis suppression should have justification")]
[SuppressMessage("Globalization", "CA1305:Укажите IFormatProvider")]
public class RandomDataSeeder: IDataSeeder
{
    private const int LocationCount = 1_000;
    private const int DepartmentCount = 5_000;
    private const int PositionCount = 4_000;
    private const int RootCount = 50;
    private const int MaxDepth = 6;
    
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<RandomDataSeeder> _logger;

    public RandomDataSeeder(DirectoryServiceDbContext dbContext, ILogger<RandomDataSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    private static readonly (string Country, string City, string Timezone)[] Cities =
    [
        ("Germany", "Berlin", "Europe/Berlin"),
        ("Germany", "Munich", "Europe/Berlin"),
        ("Germany", "Hamburg", "Europe/Berlin"),
        ("Germany", "Cologne", "Europe/Berlin"),
        ("Germany", "Frankfurt", "Europe/Berlin"),
        ("Poland", "Warsaw", "Europe/Warsaw"),
        ("Austria", "Vienna", "Europe/Vienna"),
        ("Switzerland", "Zurich", "Europe/Zurich"),
        ("Netherlands", "Amsterdam", "Europe/Amsterdam"),
        ("Czech Republic", "Prague", "Europe/Prague"),
        ("France", "Paris", "Europe/Paris"),
        ("Spain", "Madrid", "Europe/Madrid"),
    ];

    private static readonly string[] Streets =
        ["Hauptstrasse", "Bahnhofstrasse", "Gartenweg", "Schulstrasse", "Lindenallee", "Kirchweg"];

    private static readonly string[] DepartmentWords =
        ["Platform", "Backend", "Frontend", "Finance", "Sales", "Support", "Research", "Legal", "Design", "Data"];

    private static readonly string[] PositionWords =
        ["Engineer", "Manager", "Analyst", "Specialist", "Consultant", "Lead", "Coordinator", "Architect"];

    private async Task SeedDataAsync(int seed = 42, CancellationToken ct = default)
    {
        var random = new Random(seed);

        // ── 1. Локации ───────────────────────────────────────────────────
        var locations = new List<Location>(LocationCount);
        for (int i = 1; i <= LocationCount; i++)
        {
            var (country, city, timezone) = Cities[random.Next(Cities.Length)];

            locations.Add(Location.Create(
                LocationName.Convert($"{city} Office {i}"),
                Address.Convert(country, city, Streets[random.Next(Streets.Length)], i.ToString(), random.Next(10000, 99999)),
                Timezone.Convert(timezone),
                Guid.NewGuid()).Value);
        }

        // Первые 85% локаций можно привязывать к департаментам, остальные останутся пустыми.
        int assignableLocations = (int)(LocationCount * 0.85);

        _dbContext.Locations.AddRange(locations);
        await _dbContext.SaveChangesAsync(ct);
        _dbContext.ChangeTracker.Clear();

        // ── 2. Департаменты ──────────────────────────────────────────────
        var departments = new List<(Department Entity, Guid Id, int Depth)>(DepartmentCount);

        for (int i = 1; i <= DepartmentCount; i++)
        {
            var id = Guid.NewGuid();
            var name = DepartmentName.Convert($"{DepartmentWords[random.Next(DepartmentWords.Length)]} {i}");
            var identifier = Identifier.Convert($"DEP-{ToLetters(i)}");
            var departmentLocations = PickLocations(random, locations, assignableLocations, id);

            if (i <= RootCount)
            {
                var root = Department.CreateParent(name, identifier, departmentLocations, id).Value;
                departments.Add((root, id, 1));
            }
            else
            {
                // Родитель — случайный уже созданный департамент, у которого ещё есть место по глубине.
                (Department Entity, Guid Id, int Depth) parent;
                do parent = departments[random.Next(departments.Count)];
                while (parent.Depth >= MaxDepth);

                var child = Department.CreateChild(name, identifier, parent.Entity, departmentLocations, id).Value;
                departments.Add((child, id, parent.Depth + 1));
            }
        }

        _dbContext.Departments.AddRange(departments.Select(d => d.Entity));
        await _dbContext.SaveChangesAsync(ct);
        _dbContext.ChangeTracker.Clear();

        // ── 3. Позиции ───────────────────────────────────────────────────
        var positions = new List<Position>(PositionCount);
        for (int i = 1; i <= PositionCount; i++)
        {
            var id = Guid.NewGuid();

            // 10% — без департаментов, остальные — в 1–3 департаментах.
            var departmentPositions = random.NextDouble() < 0.10
                ? []
                : departments
                    .OrderBy(_ => random.Next())
                    .Take(random.Next(1, 4))
                    .Select(d => new DepartmentPosition(d.Id, id))
                    .ToList();

            positions.Add(Position.Create(
                PositionName.Convert($"{PositionWords[random.Next(PositionWords.Length)]} {i}"),
                departmentPositions,
                $"Generated position number {i}.",
                id).Value);
        }

        _dbContext.Positions.AddRange(positions);
        await _dbContext.SaveChangesAsync(ct);
        _dbContext.ChangeTracker.Clear();
    }

    private List<DepartmentLocation> PickLocations(
        Random random, List<Location> locations, int assignableCount, Guid departmentId)
    {
        int count = random.Next(1, 4);
        var picked = new HashSet<int>();
        while (picked.Count < count)
            picked.Add(random.Next(assignableCount));

        return picked
            .Select(index => new DepartmentLocation(departmentId, locations[index].Id))
            .ToList();
    }

    /// <summary>1 → "A", 26 → "Z", 27 → "AA" … — уникальный код без цифр.</summary>
    private string ToLetters(int number)
    {
        var result = string.Empty;
        while (number > 0)
        {
            number--;
            result = (char)('A' + (number % 26)) + result;
            number /= 26;
        }
        
        return result;
    }

    public async Task SeedAsync()
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            if (await _dbContext.Locations.AnyAsync()) return;

            await SeedDataAsync();
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