namespace DirectoryService.Infrastructure.Locations;

public record LocationRequestDto(string Name, string Timezone, LocationAddressDto Address);

public record LocationAddressDto(string Country, string City, string Street, string HouseNumber, int PostalCode);