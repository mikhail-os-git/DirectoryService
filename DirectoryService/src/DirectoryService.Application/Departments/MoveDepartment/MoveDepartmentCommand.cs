using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Departments.MoveDepartment;

public record MoveDepartmentCommand(Guid departmentId, Guid? parentId) : ICommand;