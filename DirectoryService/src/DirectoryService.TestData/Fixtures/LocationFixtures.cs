using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.TestData.Fixtures;

public static class LocationFixtures
{
    public static readonly Guid BerlinId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    public static readonly Guid MunichId = Guid.Parse("00000000-0000-0000-0000-000000000002");
    public static readonly Guid HamburgId = Guid.Parse("00000000-0000-0000-0000-000000000003");
    public static readonly Guid FrankfurtId = Guid.Parse("00000000-0000-0000-0000-000000000004");
    public static readonly Guid CologneId = Guid.Parse("00000000-0000-0000-0000-000000000005");
    public static readonly Guid WarsawId = Guid.Parse("00000000-0000-0000-0000-000000000006");
    public static readonly Guid ViennaId = Guid.Parse("00000000-0000-0000-0000-000000000007");
    public static readonly Guid ZurichId = Guid.Parse("00000000-0000-0000-0000-000000000008");
    public static readonly Guid AmsterdamId = Guid.Parse("00000000-0000-0000-0000-000000000009");
    public static readonly Guid PragueId = Guid.Parse("00000000-0000-0000-0000-000000000010");

    public static readonly Location Berlin = Location.Create(
        LocationName.Convert("Berlin Main"),
        Address.Convert("Germany", "Berlin", "Unter den Linden", "12", 10117),
        Timezone.Convert("Europe/Berlin"),
        BerlinId).Value;

    public static readonly Location Munich = Location.Create(
        LocationName.Convert("Munich Office"),
        Address.Convert("Germany", "Munich", "Maximilianstrasse", "5", 80539),
        Timezone.Convert("Europe/Berlin"),
        MunichId).Value;

    public static readonly Location Hamburg = Location.Create(
        LocationName.Convert("Hamburg Office"),
        Address.Convert("Germany", "Hamburg", "Moenckebergstrasse", "21", 20095),
        Timezone.Convert("Europe/Berlin"),
        HamburgId).Value;

    public static readonly Location Frankfurt = Location.Create(
        LocationName.Convert("Frankfurt Office"),
        Address.Convert("Germany", "Frankfurt", "Kaiserstrasse", "8", 60311),
        Timezone.Convert("Europe/Berlin"),
        FrankfurtId).Value;

    public static readonly Location Cologne = Location.Create(
        LocationName.Convert("Cologne Office"),
        Address.Convert("Germany", "Cologne", "Schildergasse", "3", 50667),
        Timezone.Convert("Europe/Berlin"),
        CologneId).Value;

    public static readonly Location Warsaw = Location.Create(
        LocationName.Convert("Warsaw Office"),
        Address.Convert("Poland", "Warsaw", "Nowy Swiat", "15", 10000),
        Timezone.Convert("Europe/Warsaw"),
        WarsawId).Value;

    public static readonly Location Vienna = Location.Create(
        LocationName.Convert("Vienna Office"),
        Address.Convert("Austria", "Vienna", "Kaerntner Strasse", "9", 1010),
        Timezone.Convert("Europe/Vienna"),
        ViennaId).Value;

    public static readonly Location Zurich = Location.Create(
        LocationName.Convert("Zurich Office"),
        Address.Convert("Switzerland", "Zurich", "Bahnhofstrasse", "33", 8001),
        Timezone.Convert("Europe/Zurich"),
        ZurichId).Value;

    public static readonly Location Amsterdam = Location.Create(
        LocationName.Convert("Amsterdam Office"),
        Address.Convert("Netherlands", "Amsterdam", "Damrak", "70", 1012),
        Timezone.Convert("Europe/Amsterdam"),
        AmsterdamId).Value;

    public static readonly Location Prague = Location.Create(
        LocationName.Convert("Prague Office"),
        Address.Convert("Czech Republic", "Prague", "Vaclavske namesti", "11", 11000),
        Timezone.Convert("Europe/Prague"),
        PragueId).Value;

    public static IReadOnlyList<Location> All =>
    [
        Berlin, Munich, Hamburg, Frankfurt, Cologne,
        Warsaw, Vienna, Zurich, Amsterdam, Prague
    ];
}
