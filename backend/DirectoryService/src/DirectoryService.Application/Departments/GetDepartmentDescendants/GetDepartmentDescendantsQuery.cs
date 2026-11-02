using DirectoryService.Contracts.Common;
using Shared.Core.Abstractions;

namespace DirectoryService.Application.Departments.GetDepartmentDescendants;

public record GetDepartmentDescendantsQuery(Guid DepartmentId, PageSettings PageSettings): IQuery;