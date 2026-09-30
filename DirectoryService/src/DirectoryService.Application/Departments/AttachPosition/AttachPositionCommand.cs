using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Departments.AttachPosition;

public record AttachPositionCommand(Guid DepartmentId, Guid PositionId) : ICommand;