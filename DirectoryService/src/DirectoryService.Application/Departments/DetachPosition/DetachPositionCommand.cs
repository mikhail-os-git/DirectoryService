using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Departments.DetachPosition;

public record DetachPositionCommand(Guid DepartmentId, Guid PositionId) : ICommand;