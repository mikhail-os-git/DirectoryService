namespace DirectoryService.Infrastructure.Departments;

public interface IDepartmentsRepository
{
    Task<Guid> AddAsync(Department department, CancellationToken cancellationToken);
    
    Task<Guid> SaveAsync(Department department, CancellationToken cancellationToken);
    
    Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken);
    
    Task<Guid> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken);
}