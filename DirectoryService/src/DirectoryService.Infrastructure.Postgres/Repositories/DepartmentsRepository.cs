using System.Linq.Expressions;
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
        
        // _logger.LogInformation("Department {Id} created successfully", department.Id);
        return department.Id;
    }

    public async Task<Department?> GetByAsync(
        Expression<Func<Department, bool>> expression,
        CancellationToken cancellationToken)
    {
        return await _context.Departments.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<bool> IsMatchAsync(Expression<Func<Department, bool>> expression, CancellationToken cancellationToken)
    {
        return await _context.Departments.AnyAsync(expression, cancellationToken);
    }

    public async Task<bool> AllMatchAsync(IEnumerable<Guid> ids, Expression<Func<Department, bool>> expression,
        CancellationToken cancellationToken)
    {
        List<Guid> collection = ids.ToList();

        int count = await _context.Departments.Where(expression).CountAsync(cancellationToken);
        return collection.Count == count;
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
    
    // public Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken) => throw new NotImplementedException();
}
