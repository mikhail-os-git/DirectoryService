using Shared.Core.Abstractions;

namespace DirectoryService.Application.Departments.MoveDepartment;

public record MoveDepartmentCommand(Guid DepartmentId, Guid? ParentId) : ICommand;