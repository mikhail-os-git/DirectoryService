using DirectoryService.Contracts.Common;
using Shared.Core.Abstractions;

namespace DirectoryService.Application.Departments.GetDepartmentAncestors;

public record GetDepartmentAncestorsQuery(Guid DepartmentId, PageSettings PageSettings): IQuery;