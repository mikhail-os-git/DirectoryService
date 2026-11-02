using DirectoryService.Contracts.Common;
using Shared.Core.Abstractions;

namespace DirectoryService.Application.Departments.GetTreeByQueryDepartments;

public record GetTreeByQueryDepartmentsQuery(string Search, PageSettings PageSettings) : IQuery;