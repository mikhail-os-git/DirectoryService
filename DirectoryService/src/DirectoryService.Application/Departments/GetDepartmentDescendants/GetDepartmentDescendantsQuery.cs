using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Common;

namespace DirectoryService.Application.Departments.GetDepartmentDescendants;

public record GetDepartmentDescendantsQuery(Guid DepartmentId, PageSettings PageSettings): IQuery;