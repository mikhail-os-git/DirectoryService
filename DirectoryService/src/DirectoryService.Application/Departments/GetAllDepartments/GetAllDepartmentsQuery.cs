using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Departments.GetAllDepartments;

namespace DirectoryService.Application.Departments.GetAllDepartments;

public record GetAllDepartmentsQuery(GetAllDepartmentsRequest Request): IQuery;