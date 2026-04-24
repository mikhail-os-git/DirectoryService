using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Domain.Departments;
using General.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Repositories;

public class DepartmentsRepository: IDepartmentsRepository
{
    private readonly DirectoryServiceDbContext _context;
    private readonly ILogger<DepartmentsRepository> _logger;

    public DepartmentsRepository(DirectoryServiceDbContext context, ILogger<DepartmentsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Guid, Failure>> AddAsync(Department department, CancellationToken cancellationToken)
    {
        await _context.Departments.AddAsync(department, cancellationToken);

        var result = await SaveAsync(cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        _logger.LogInformation("Department {Id} created successfully", department.Id);
        return department.Id;
    }

    public async Task<bool> AllDepartmentsExistAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        List<Guid> collection = ids.ToList();
        int foundCount = await _context.Departments
            .CountAsync(d => collection.Contains(d.Id), cancellationToken);

        return foundCount == collection.Count;
    }

    public async Task<bool> AllDepartmentsIsActiveAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        List<Guid> collection = ids.ToList();
        return !await _context.Departments.AnyAsync(d => collection.Contains(d.Id) && !d.IsActive, cancellationToken);
    }

    public async Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Failure>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to save changes: {Error}", ex);
            return UnitResult.Failure<Failure>(Failure.Error("Something went wrong", "server.internal"));
        }
    }

    public async Task<Result<Department, Failure>> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);

        if (department == null)
            return Failure.NotFoundEntity("Department not found", departmentId, "department.not.found");

        return department;
    }
    
    // public Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken) => throw new NotImplementedException();
}
