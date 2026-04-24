using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments;
using General.Errors;

namespace DirectoryService.Application.Departments.Interfaces;

public interface IDepartmentsRepository
{
    Task<Result<Guid, Failure>> AddAsync(Department department, CancellationToken cancellationToken);
        
    Task<Result<Department, Failure>> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken);
    
    Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken);

    Task<bool> AllDepartmentsExistAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);

    Task<bool> AllDepartmentsIsActiveAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    
    // Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken);
}