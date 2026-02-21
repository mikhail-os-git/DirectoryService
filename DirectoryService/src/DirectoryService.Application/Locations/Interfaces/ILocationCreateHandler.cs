using CSharpFunctionalExtensions;

namespace DirectoryService.Infrastructure.Locations.Interfaces;

public interface ILocationCreateHandler
{
    Task<Result<Guid, string>> Handle(LocationRequestDto request, CancellationToken cancellationToken);
}