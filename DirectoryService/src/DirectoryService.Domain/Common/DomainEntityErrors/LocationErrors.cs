using General.Errors;

namespace DirectoryService.Domain.Common.DomainEntityErrors;

public static class LocationErrors
{
    private const string Entity = "Location";
    private const string EntityPlural = "Locations";

    public static Failure NotFound(Guid locationId) =>
        CommonErrors.EntityNotFound(Entity, locationId);

    public static Failure CollectionNotFound(IEnumerable<Guid> locationIds) =>
        CommonErrors.EntityCollectionNotFound(EntityPlural, locationIds);

    public static Failure PropertyConflict(string value, string property = "") =>
        CommonErrors.EntityConflictProperty(Entity, property, value);
    
    public static Failure EntityConflict(Guid id) =>
        CommonErrors.EntityConflict(Entity, id);

    public static Failure Inactive(Guid locationId) =>
        CommonErrors.EntityInactive(Entity, locationId);

    public static Failure CollectionInactive(IEnumerable<Guid> locationIds) =>
        CommonErrors.EntityCollectionInactive(EntityPlural, locationIds);
}