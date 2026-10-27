using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Common;

namespace DirectoryService.Application.Departments.GetDepartmentAncestors;

public record GetDepartmentAncestorsQuery(Guid DepartmentId, PageSettings PageSettings): IQuery;