using Shared.Core.Abstractions;

namespace DirectoryService.Application.Departments.HardDeleteDepartment;

public record HardDeleteDepartmentCommand(Guid DepartmentId): ICommand;