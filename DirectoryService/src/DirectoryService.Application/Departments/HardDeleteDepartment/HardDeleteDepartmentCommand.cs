using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Departments.HardDeleteDepartment;

public record HardDeleteDepartmentCommand(Guid DepartmentId): ICommand;