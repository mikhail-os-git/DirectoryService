using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments;
using General.Errors;

namespace DirectoryService.Application.Departments.Interfaces;

public interface IDepartmentsRepository
{
    Task<Result<Guid, Failure>> AddAsync(Department department, CancellationToken cancellationToken);
        
    Task<Department?> GetByAsync(Expression<Func<Department, bool>> expression, CancellationToken cancellationToken);

    Task<bool> IsMatchAsync(Expression<Func<Department, bool>> expression, CancellationToken cancellationToken);
    
    Task<bool> AllMatchAsync(IEnumerable<Guid> ids, Expression<Func<Department, bool>> expression,
        CancellationToken cancellationToken);

    Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken);
    
    // Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken);
}