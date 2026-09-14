using System.Text.Json.Serialization;

namespace DirectoryService.Contracts.Common;

public record PagedResult<T>
{
    [JsonInclude]
    public IReadOnlyList<T> Items = [];
    public long TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }

    public PagedResult(IEnumerable<T> items, long? totalCount, int page, int pageSize)
    {
        TotalCount = totalCount ?? 0;
        Page = page;
        PageSize = pageSize;
        Items = items is not null ? items.ToList().AsReadOnly() : [];
    }
}