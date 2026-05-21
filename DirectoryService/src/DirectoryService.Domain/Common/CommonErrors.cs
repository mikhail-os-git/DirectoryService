using System.Globalization;
using General.Errors;

namespace DirectoryService.Domain.Common;

public static class CommonErrors
{
    public static Failure InternalError =>
        Failure.Error("Something went wrong", "server.internal");
    
    public static Failure ConcurrencyConflict => Failure.Conflict("Conflict detected, please try again", "concurrency.conflict");

    public static Failure EntityNotFound(string entity, Guid id) =>
        Failure.NotFoundEntity(
            $"{entity} not found",
            id,
            $"{entity.ToLower(CultureInfo.InvariantCulture)}.not_found");

    public static Failure EntityCollectionNotFound(string entity, IEnumerable<Guid> ids) =>
        Failure.NotFoundCollectionEntity(
            $"One or more {entity} not found: {string.Join(", ", ids)}",
            $"{entity.ToLower(CultureInfo.InvariantCulture)}.not_found");

    public static Failure EntityConflictProperty(string entity, string property, string value) =>
        Failure.Conflict(
            $"{entity} with {property} '{value}' already exists",
            $"{entity.ToLower(CultureInfo.InvariantCulture)}.{property.ToLower(CultureInfo.InvariantCulture)}.conflict");
    
    public static Failure EntityConflict(string entity, Guid id) =>
        Failure.ConflictEntity(
            $"{entity} already exists",
            $"{entity.ToLower(CultureInfo.InvariantCulture)}.{entity.ToLower(CultureInfo.InvariantCulture)}.conflict",
            id);
    
    public static Failure EntityInactive(string entity, Guid id) =>
        Failure.Conflict(
            $"{entity} with id '{id}' is inactive",
            $"{entity.ToLower(CultureInfo.InvariantCulture)}.inactive");

    public static Failure EntityCollectionInactive(string entity, IEnumerable<Guid> ids) =>
        Failure.Conflict(
            $"One or more {entity} are inactive: {string.Join(", ", ids)}",
            $"{entity.ToLower(CultureInfo.InvariantCulture)}.inactive");

    public static Failure CollectionItemsInvalid(string collectionName) => Failure.Validation(
        $"Collection '{collectionName}' contains invalid items.",
        $"{collectionName.ToLower(CultureInfo.InvariantCulture)}.collection.invalid");
    
    public static Failure UniqueCollectionInvalid(string collectionName) => Failure.Validation(
            "All items in the collection must be unique.",
            $"{collectionName.ToLower(CultureInfo.InvariantCulture)}.collection.invalid");
    public static Failure CollectionEmpty(string collectionName) => Failure.Validation("The collection should not be empty.", $"{collectionName.ToLower(CultureInfo.InvariantCulture)}.collection.invalid");
    
}