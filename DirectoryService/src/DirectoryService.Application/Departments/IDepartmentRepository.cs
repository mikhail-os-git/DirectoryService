using DirectoryService.Domain.Departments;

namespace DirectoryService.Application.Departments;

public interface IDepartmentRepository
{
    Task<Guid> AddAsync(Department department, CancellationToken cancellationToken);
    
    Task<Guid> SaveAsync(Department department, CancellationToken cancellationToken);
    
    Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken);
    
    Task<Guid> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken);
}