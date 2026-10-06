namespace DirectoryService.Domain.Common.Constants;

public static class ApplicationConstants
{
    public const string NPGSQL_SEARCH_APPLIER_KEY = "postgress";
    public const string WORKER_SETTINGS = "WorkersSettings";
    public static readonly CleanupKeys CLEANUP_KEYS = new CleanupKeys();
}

public record CleanupKeys
{
    public string DEPARTMENTS { get; } = "Departments";
    public string POSITIONS { get; } = "Positions";
    public string LOCATIONS { get; } = "Locations";
}