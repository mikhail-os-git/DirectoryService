using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Departments;
using General.Errors;

namespace DirectoryService.Application.Departments.Interfaces;

public interface IDepartmentsRepository
{
    Task<Guid> AddAsync(Department department, CancellationToken cancellationToken);
        
    Task<Department?> GetByAsync(Expression<Func<Department, bool>> expression, CancellationToken cancellationToken);

    Task<Department?> GetByIdWithLockAsync(Guid departmentId, CancellationToken cancellationToken);
    
    Task<UnitResult<Failure>> LockDescendantsAsync(string path, CancellationToken cancellationToken);
    
    Task<bool> IsMatchAsync(Expression<Func<Department, bool>> expression, CancellationToken cancellationToken);
    
    Task<bool> AllMatchAsync(IEnumerable<Guid> ids, Expression<Func<Department, bool>> expression,
        CancellationToken cancellationToken);
    Task<bool> IsDescendantOfAsync(string potentialDescendantPath, string ancestorPath, CancellationToken cancellationToken);

    Task<bool> HasDescendantsAsync(string path, CancellationToken cancellationToken);
    
    Task<UnitResult<Failure>> DeleteDepartmentLocationsByIdAsync(Guid departmentId, CancellationToken cancellationToken);

    Task<Result<string?, Failure>> MoveDepartmentAsync(string oldChildPath, string parentPath,
        CancellationToken cancellationToken);

    Task<UnitResult<Failure>> MoveDescendantsAsync(string oldPath, string newPath, CancellationToken cancellationToken);

    /// <summary>
    /// Физически удаляет подразделение. Дочерние подразделения становятся корневыми
    /// (parent_id = NULL), пути и глубина поддерева пересчитываются.
    /// </summary>
    /// <remarks>
    /// Намеренно работает без фильтра по is_active (Dapper не применяет query filters EF):
    /// проверка «только активные» выполняется в handler-е, а дети обрабатываются все,
    /// иначе FK не даст удалить строку.
    /// Перед пересчётом поддерево блокируется (FOR UPDATE).
    /// Требует активной транзакции от TransactionManager.
    /// </remarks>
    Task<Result<int, Failure>> HardDeleteAsync(Department department, CancellationToken cancellationToken);
    
    Task<bool> CheckAttachedPosition(
        Expression<Func<DepartmentPosition, bool>> expression,
        CancellationToken cancellationToken);

    Task AttachPosition(Guid depId, Guid posId, CancellationToken cancellationToken);

    Task<int> DetachPosition(
        Expression<Func<DepartmentPosition, bool>> expression,
        CancellationToken cancellationToken);
    
    // Task<Guid> AddDepartmentLocationsAsync(
    //     IEnumerable<DepartmentLocation> departmentLocations,
    //     CancellationToken cancellationToken);
    //
    // Task<Guid> AddDepartmentLocationsAsync(
    //     Guid departmentId,
    //     IEnumerable<Guid> locationIds,
    //     CancellationToken cancellationToken);
}