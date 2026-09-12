using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;
using General;
using General.Errors;

namespace DirectoryService.Domain.ValueObjects;

public record Address
{
    public string Country { get; } = null!;
    public string City { get; } = null!;
    public string Street { get; } = null!;
    public string HouseNumber { get; } = null!;
    public int PostalCode { get; }

    private const char MAIN_SEPARATOR = ',';
    private const char SECOND_SEPARATOR = ':'; 
    
    // EF Core
    private Address()
    {
    }
    
    private Address(string country, string city, string street, string houseNumber, int postalCode)
    {
        Country = country;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        PostalCode = postalCode;
    }

    public static Result<Address, Failure> Create(string country, string city, string street, string houseNumber,
        int postalCode)
    {
        string houseNumberRegex = @"^[A-Za-z0-9\u0400-\u04FF/\-\.]+$";
        bool notEmptyInvalidHouseNumber = false;
        bool invalidPostalCode = false;
        List<string> fields = [];

        if (StringValidator.IsEmpty(country))
        {
            fields.Add("Country");
        }
        
        if (StringValidator.IsEmpty(city))
        {
            fields.Add("City");
        }

        if (StringValidator.IsEmpty(street))
        {
            fields.Add("Street");
        }

        if (StringValidator.IsEmpty(houseNumber))
        {
            fields.Add("House Number");
        }
        else
        {
            if (!Regex.IsMatch(houseNumber, houseNumberRegex))
            {
                fields.Add("House Number");
                notEmptyInvalidHouseNumber = true;
            }
        }

        if (postalCode < 0)
        {
            invalidPostalCode = true;
            fields.Add("Postal Code");
        }

        if (fields.Count > 0)
        {
             string message = $"the data {string.Join(", ", fields)} was not specified or entered. " +
                   $"{(notEmptyInvalidHouseNumber ? "Incorrect house number." : string.Empty)}" +
                   $"{(invalidPostalCode ? "The postal code is incorrect" : string.Empty)}";

             return Failure.Validation(message, "address.is.invalid");
        }

        return new Address(country, city, street, houseNumber, postalCode);
    }

    public override string ToString()
    {
        return Serialize(this);
    }

    public static string Serialize(Address address)
    {
        return $"{nameof(Country)}{SECOND_SEPARATOR} {address.Country}{MAIN_SEPARATOR} " +
               $"{nameof(City)}{SECOND_SEPARATOR} {address.City}{MAIN_SEPARATOR} " +
               $"{nameof(Street)}{SECOND_SEPARATOR} {address.Street}{MAIN_SEPARATOR} " +
               $"{nameof(HouseNumber)}{SECOND_SEPARATOR} {address.HouseNumber}{MAIN_SEPARATOR} " +
               $"{nameof(PostalCode)}{SECOND_SEPARATOR} {address.PostalCode}";   
    }

    public static Address Deserialize(string addressString)
    {
        string[] rows = addressString.Split(MAIN_SEPARATOR);
        if (rows.Length < 4)
            throw new FormatException($"Invalid failure string format: '{addressString}'");
        
        Dictionary<string, string> addressDictionary = new();
        foreach (var row in rows)
        {
            string[] splited = row.Split(SECOND_SEPARATOR, 2);
            string key = splited[0].Trim();

            if (key != nameof(Country) &&
                key != nameof(City) &&
                key != nameof(Street) &&
                key != nameof(HouseNumber) &&
                key != nameof(PostalCode))
            {
                throw new FormatException($"Unknown field '{key}' in failure string: '{addressString}'");
            }
            
            addressDictionary.Add(key, splited[1].Trim());
        }

        if (!int.TryParse(addressDictionary[nameof(PostalCode)], out int postalCode))
        {
            throw new ArgumentException($"Unknown FailureType value: '{addressDictionary[nameof(PostalCode)]}' - {addressDictionary[nameof(PostalCode)]}");
        }

        Address address = new(
            addressDictionary[nameof(Country)],
            addressDictionary[nameof(City)],
            addressDictionary[nameof(Street)],
            addressDictionary[nameof(HouseNumber)],
            postalCode);

        return address;
    }
    
    /// <summary>
    /// Создаёт экземпляр без доменной валидации.
    /// Только для десериализации в инфраструктурном слое (EF Core и т.п.).
    /// Для создания из пользовательского ввода используй <see cref="Create"/>.
    /// </summary>
    /// <param name="country">Страна, прочитанная из источника данных.</param>
    /// <param name="city">Город, прочитанный из источника данных.</param>
    /// <param name="street">Улица, прочитанная из источника данных.</param>
    /// <param name="houseNumber">Номер дома, прочитанный из источника данных.</param>
    /// <param name="postalCode">Почтовый индекс, прочитанный из источника данных.</param>
    /// <returns>Экземпляр <see cref="Address"/>.</returns>
    public static Address Convert(string country, string city, string street, string houseNumber, int postalCode) =>
        new(country, city, street, houseNumber, postalCode);

    public static implicit operator string(Address address) => address.ToString();
}