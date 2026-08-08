namespace DirectoryService.Contracts.Locations.CreateLocation;

public record CreateLocationRequest(string Name, string Timezone, AddressRequest Address);

public record AddressRequest(string Country, string City, string Street, string HouseNumber, int PostalCode);