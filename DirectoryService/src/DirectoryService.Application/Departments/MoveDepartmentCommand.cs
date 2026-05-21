using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Departments;

public record MoveDepartmentCommand(Guid departmentId, Guid? parentId) : ICommand;