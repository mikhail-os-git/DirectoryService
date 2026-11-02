using DirectoryService.Contracts.Departments.GetAllDepartments;
using Shared.Core.Abstractions;

namespace DirectoryService.Application.Departments.GetAllDepartments;

public record GetAllDepartmentsQuery(GetAllDepartmentsRequest Request): IQuery;