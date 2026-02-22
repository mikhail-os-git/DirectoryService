namespace DirectoryService.Contracts.Locations;

public record LocationRequest(string Name, string Timezone, AddressRequest Address);

public record AddressRequest(string Country, string City, string Street, string HouseNumber, int PostalCode);