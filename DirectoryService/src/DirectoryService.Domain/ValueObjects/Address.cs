using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Common;

namespace DirectoryService.Domain.ValueObjects;

public record Address
{
    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string HouseNumber { get; }
    public int PostalCode { get; }

    public Address(string country, string city, string street, string houseNumber, int postalCode)
    {
        Country = country;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        PostalCode = postalCode;
    }

    public static Result<Address, string> Create(string country, string city, string street, string houseNumber,
        int postalCode)
    {
        string houseNumberRegex = @"^[A-Za-z0-9/\-\.]+$";
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
            return $"the data {string.Join(", ", fields)} was not specified or entered. " +
                   $"{(notEmptyInvalidHouseNumber ? "Incorrect house number." : string.Empty)}" +
                   $"{(invalidPostalCode ? "The postal code is incorrect" : string.Empty)}";
        }

        return new Address(country, city, street, houseNumber, postalCode);
    }

    public override string ToString()
    {
        return $"{nameof(Country)}: {Country} ,{nameof(City)}: {City}, {nameof(Street)}: {Street}, {nameof(HouseNumber)}: {HouseNumber}, {nameof(PostalCode)}: {PostalCode}.";
    } 
}