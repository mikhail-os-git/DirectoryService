using DirectoryService.Contracts.Departments;
using DirectoryService.Contracts.Departments.CreateDepartment;
using Shared.Core.Abstractions;

namespace DirectoryService.Application.Departments.CreateDepartment;

public record CreateDepartmentCommand(CreateDepartmentRequest Request) : ICommand;