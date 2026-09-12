using System.Text.Json.Serialization;

namespace DirectoryService.Contracts.Common;

public record PagedResult<T>
{
    [JsonInclude]
    public IReadOnlyList<T> Items = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }

    public PagedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
        Items = items is not null ? items.ToList().AsReadOnly() : [];
    }
}