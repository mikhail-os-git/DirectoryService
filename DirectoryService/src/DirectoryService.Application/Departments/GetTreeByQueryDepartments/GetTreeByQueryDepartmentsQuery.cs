using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Common;

namespace DirectoryService.Application.Departments.GetTreeByQueryDepartments;

public record GetTreeByQueryDepartmentsQuery(string Search, PageSettings PageSettings) : IQuery;