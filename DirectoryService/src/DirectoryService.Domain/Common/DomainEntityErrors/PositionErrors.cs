using General.Errors;

namespace DirectoryService.Domain.Common.DomainEntityErrors;

public static class PositionErrors
{
    private const string Entity = "Position";
    private const string EntityPlural = "Positions";

    public static Failure NotFound(Guid positionId) =>
        CommonErrors.EntityNotFound(Entity, positionId);

    public static Failure CollectionNotFound(IEnumerable<Guid> positionIds) =>
        CommonErrors.EntityCollectionNotFound(EntityPlural, positionIds);

    public static Failure PropertyConflict(string value, string property = "") =>
        CommonErrors.EntityConflictProperty(Entity, property, value);

    public static Failure EntityConflict(Guid id) =>
        CommonErrors.EntityConflict(Entity, id);
    
    public static Failure Inactive(Guid positionId) =>
        CommonErrors.EntityInactive(Entity, positionId);

    public static Failure CollectionInactive(IEnumerable<Guid> positionIds) =>
        CommonErrors.EntityCollectionInactive(EntityPlural, positionIds);
}