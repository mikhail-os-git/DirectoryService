using DirectoryService.Contracts.Common;
using Shared.Core.Abstractions;

namespace DirectoryService.Application.Departments.GetTreeDepartments;

public record GetTreeDepartmentsQuery(PageSettings PageSettings): IQuery;