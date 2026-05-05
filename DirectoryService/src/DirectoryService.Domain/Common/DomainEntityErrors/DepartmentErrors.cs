using General.Errors;

namespace DirectoryService.Domain.Common.DomainEntityErrors;

public static class DepartmentErrors
{
    private const string Entity = "Department";
    private const string EntityPlural = "Departments";

    public static Failure NotFound(Guid departmentId) =>
        CommonErrors.EntityNotFound(Entity, departmentId);

    public static Failure CollectionNotFound(IEnumerable<Guid> departmentIds) =>
        CommonErrors.EntityCollectionNotFound(EntityPlural, departmentIds);

    public static Failure PropertyConflict(string value, string property = "") =>
        CommonErrors.EntityConflictProperty(Entity, property, value);
    
    public static Failure EntityConflict(Guid id) =>
        CommonErrors.EntityConflict(Entity, id);

    public static Failure Inactive(Guid departmentId) =>
        CommonErrors.EntityInactive(Entity, departmentId);

    public static Failure CollectionInactive(IEnumerable<Guid> departmentIds) =>
        CommonErrors.EntityCollectionInactive(EntityPlural, departmentIds);
}