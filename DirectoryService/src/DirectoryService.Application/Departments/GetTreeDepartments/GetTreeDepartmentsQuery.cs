using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Common;

namespace DirectoryService.Application.Departments.GetDepartmentsTree;

public record GetTreeDepartmentsQuery(PageSettings PageSettings): IQuery;