using DirectoryService.Contracts.Common;

namespace DirectoryService.Contracts.Departments.GetAllDepartments;

public record GetAllDepartmentsRequest
{
    public PageSettings PageSettings { get; init; } = null!;
    public string Search { get; init; } = null!;
    public string SortBy { get; init; } = "name";
    public string SortDir { get; init; } = "asc";
}