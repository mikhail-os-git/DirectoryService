using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Common;
using DirectoryService.Contracts.Locations.GetLocations;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Locations.GetLocations;

public class GetLocationsHandler: IQueryHandler<PagedResult<GetLocationsResponseItem>, GetLocationsQuery>
{
    private readonly IDirectoryReadDbContext _readDbContext;
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IValidator<GetLocationsRequest> _validator;

    public GetLocationsHandler(
        IDirectoryReadDbContext readDbContext, 
        IDbConnectionFactory connectionFactory,
        IValidator<GetLocationsRequest> validator)
    {
        _readDbContext = readDbContext;
        _connectionFactory = connectionFactory;
        _validator = validator;
    }
    
    public async Task<Result<PagedResult<GetLocationsResponseItem>, FailList>> Handle(
        GetLocationsQuery query,
        CancellationToken cancellationToken)
    {
        var validate = await _validator.ValidateAsync(query.Request, cancellationToken);
        if (!validate.IsValid)
            return validate.ToFailList();

        using var connecntion = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        long? totalCount = null;
        
        var parameters = new DynamicParameters();
        parameters.Add("dep_count", query.Request.DepartmentCount, DbType.Int32);
        parameters.Add("order", query.Request.SortBy, DbType.String);
        parameters.Add("offset", (query.Request.PageSettings.Page - 1) * query.Request.PageSettings.PageSize, DbType.Int32);
        parameters.Add("page_size", query.Request.PageSettings.PageSize, DbType.Int32);
        
        string where = "dl.department_count >= @dep_count";
        if (!string.IsNullOrWhiteSpace(query.Request.Search))
        {
            where += "and l.name ilike '%' || @search || '%'";
            parameters.Add("search", query.Request.Search, DbType.String);
        }
        
        var sql = $"""
                   with dep_loc as ( 
                     select location_id, count(*) as department_count
                     from department_location
                     group by location_id
                   )
                   select id, name, created_at, address, count(*) over() as total_count from locations as l
                   join dep_loc as dl on l.id = dl.location_id
                   where {where}
                   order by @order {query.Request.SortDir}
                   limit @page_size offset @offset
                   """;
        
        var items = await connecntion.QueryAsync<GetLocationsResponseItem, string, long, GetLocationsResponseItem>(
            sql, 
            splitOn: "address, total_count", 
            map:
            (item, address, count) =>
            {
                if (totalCount is null)
                {
                    totalCount = count;
                }

                item.Address = AddressNormalize(address);
                
                return item;
            },
            param: parameters);
        
        return new PagedResult<GetLocationsResponseItem>(items, totalCount, query.Request.PageSettings.Page,
            query.Request.PageSettings.PageSize);
    }

    private string AddressNormalize(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return json;
        
        var raw = JsonSerializer.Deserialize<AddressJsonRaw>(json);
        if (raw is null)
            return string.Empty;
        
        return Address.Convert(raw.Country, raw.City, raw.Street, raw.HouseNumber, raw.PostalCode);
    }
}

public class AddressJsonRaw
{
    [JsonPropertyName("city")]
    public string City { get; set; } = null!;

    [JsonPropertyName("street")]
    public string Street { get; set; } = null!;

    [JsonPropertyName("country")]
    public string Country { get; set; } = null!;

    [JsonPropertyName("postal_code")]
    public int PostalCode { get; set; }

    [JsonPropertyName("house_number")]
    public string HouseNumber { get; set; } = null!;
}