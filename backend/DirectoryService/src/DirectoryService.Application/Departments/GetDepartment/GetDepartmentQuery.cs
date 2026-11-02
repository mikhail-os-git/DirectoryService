using Shared.Core.Abstractions;

namespace DirectoryService.Application.Departments.GetDepartment;

public record GetDepartmentQuery(Guid DepartmentId): IQuery;