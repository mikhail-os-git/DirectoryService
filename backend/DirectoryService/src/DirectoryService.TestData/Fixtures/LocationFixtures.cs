using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.TestData.Fixtures;

public static class LocationFixtures
{
    // ── IDs ──────────────────────────────────────────────────────────────
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
    public static readonly Guid DresdenId = Guid.Parse("00000000-0000-0000-0000-000000000011");
    public static readonly Guid LeipzigId = Guid.Parse("00000000-0000-0000-0000-000000000012");
    public static readonly Guid StuttgartId = Guid.Parse("00000000-0000-0000-0000-000000000013");
    public static readonly Guid DuesseldorfId = Guid.Parse("00000000-0000-0000-0000-000000000014");
    public static readonly Guid HanoverId = Guid.Parse("00000000-0000-0000-0000-000000000015");
    public static readonly Guid NurembergId = Guid.Parse("00000000-0000-0000-0000-000000000016");
    public static readonly Guid BremenId = Guid.Parse("00000000-0000-0000-0000-000000000017");
    public static readonly Guid EssenId = Guid.Parse("00000000-0000-0000-0000-000000000018");
    public static readonly Guid BonnId = Guid.Parse("00000000-0000-0000-0000-000000000019");
    public static readonly Guid KarlsruheId = Guid.Parse("00000000-0000-0000-0000-000000000020");
    public static readonly Guid KrakowId = Guid.Parse("00000000-0000-0000-0000-000000000021");
    public static readonly Guid WroclawId = Guid.Parse("00000000-0000-0000-0000-000000000022");
    public static readonly Guid GrazId = Guid.Parse("00000000-0000-0000-0000-000000000023");
    public static readonly Guid GenevaId = Guid.Parse("00000000-0000-0000-0000-000000000024");
    public static readonly Guid RotterdamId = Guid.Parse("00000000-0000-0000-0000-000000000025");
    public static readonly Guid BrnoId = Guid.Parse("00000000-0000-0000-0000-000000000026");
    public static readonly Guid ParisId = Guid.Parse("00000000-0000-0000-0000-000000000027");
    public static readonly Guid LyonId = Guid.Parse("00000000-0000-0000-0000-000000000028");
    public static readonly Guid MadridId = Guid.Parse("00000000-0000-0000-0000-000000000029");
    public static readonly Guid BarcelonaId = Guid.Parse("00000000-0000-0000-0000-000000000030");
    public static readonly Guid LisbonId = Guid.Parse("00000000-0000-0000-0000-000000000031");
    public static readonly Guid PortoId = Guid.Parse("00000000-0000-0000-0000-000000000032");
    public static readonly Guid TurinId = Guid.Parse("00000000-0000-0000-0000-000000000033");
    public static readonly Guid MilanId = Guid.Parse("00000000-0000-0000-0000-000000000034");
    public static readonly Guid BrusselsId = Guid.Parse("00000000-0000-0000-0000-000000000035");
    public static readonly Guid LuxembourgId = Guid.Parse("00000000-0000-0000-0000-000000000036");
    public static readonly Guid CopenhagenId = Guid.Parse("00000000-0000-0000-0000-000000000037");
    public static readonly Guid StockholmId = Guid.Parse("00000000-0000-0000-0000-000000000038");
    public static readonly Guid GothenburgId = Guid.Parse("00000000-0000-0000-0000-000000000039");
    public static readonly Guid BergenId = Guid.Parse("00000000-0000-0000-0000-000000000040");
    public static readonly Guid TampereId = Guid.Parse("00000000-0000-0000-0000-000000000041");
    public static readonly Guid BucharestId = Guid.Parse("00000000-0000-0000-0000-000000000042");
    public static readonly Guid BudapestId = Guid.Parse("00000000-0000-0000-0000-000000000043");
    public static readonly Guid BratislavaId = Guid.Parse("00000000-0000-0000-0000-000000000044");
    public static readonly Guid LjubljanaId = Guid.Parse("00000000-0000-0000-0000-000000000045");
    public static readonly Guid ZagrebId = Guid.Parse("00000000-0000-0000-0000-000000000046");
    public static readonly Guid TallinnId = Guid.Parse("00000000-0000-0000-0000-000000000047");
    public static readonly Guid RigaId = Guid.Parse("00000000-0000-0000-0000-000000000048");
    public static readonly Guid VilniusId = Guid.Parse("00000000-0000-0000-0000-000000000049");
    public static readonly Guid AthensId = Guid.Parse("00000000-0000-0000-0000-000000000050");

    // ── Germany ──────────────────────────────────────────────────────────
    public static readonly Location Berlin = Create("Berlin Main", "Germany", "Berlin", "Unter den Linden", "12", 10117, "Europe/Berlin", BerlinId);
    public static readonly Location Munich = Create("Munich Office", "Germany", "Munich", "Maximilianstrasse", "5", 80539, "Europe/Berlin", MunichId);
    public static readonly Location Hamburg = Create("Hamburg Office", "Germany", "Hamburg", "Moenckebergstrasse", "21", 20095, "Europe/Berlin", HamburgId);
    public static readonly Location Frankfurt = Create("Frankfurt Office", "Germany", "Frankfurt", "Kaiserstrasse", "8", 60311, "Europe/Berlin", FrankfurtId);
    public static readonly Location Cologne = Create("Cologne Office", "Germany", "Cologne", "Schildergasse", "3", 50667, "Europe/Berlin", CologneId);
    public static readonly Location Dresden = Create("Dresden Office", "Germany", "Dresden", "Prager Strasse", "4", 1069, "Europe/Berlin", DresdenId);
    public static readonly Location Leipzig = Create("Leipzig Office", "Germany", "Leipzig", "Grimmaische Strasse", "2", 4109, "Europe/Berlin", LeipzigId);
    public static readonly Location Stuttgart = Create("Stuttgart Office", "Germany", "Stuttgart", "Koenigstrasse", "28", 70173, "Europe/Berlin", StuttgartId);
    public static readonly Location Duesseldorf = Create("Duesseldorf Office", "Germany", "Duesseldorf", "Koenigsallee", "60", 40212, "Europe/Berlin", DuesseldorfId);
    public static readonly Location Hanover = Create("Hanover Office", "Germany", "Hanover", "Georgstrasse", "14", 30159, "Europe/Berlin", HanoverId);
    public static readonly Location Nuremberg = Create("Nuremberg Office", "Germany", "Nuremberg", "Karolinenstrasse", "17", 90402, "Europe/Berlin", NurembergId);
    public static readonly Location Bremen = Create("Bremen Office", "Germany", "Bremen", "Obernstrasse", "30", 28195, "Europe/Berlin", BremenId);
    public static readonly Location Essen = Create("Essen Office", "Germany", "Essen", "Kettwiger Strasse", "22", 45127, "Europe/Berlin", EssenId);
    public static readonly Location Bonn = Create("Bonn Office", "Germany", "Bonn", "Poststrasse", "7", 53111, "Europe/Berlin", BonnId);
    public static readonly Location Karlsruhe = Create("Karlsruhe Office", "Germany", "Karlsruhe", "Kaiserstrasse", "120", 76133, "Europe/Berlin", KarlsruheId);

    // ── DACH / Benelux / CEE ─────────────────────────────────────────────
    public static readonly Location Warsaw = Create("Warsaw Office", "Poland", "Warsaw", "Nowy Swiat", "15", 10000, "Europe/Warsaw", WarsawId);
    public static readonly Location Krakow = Create("Krakow Office", "Poland", "Krakow", "Florianska", "24", 31019, "Europe/Warsaw", KrakowId);
    public static readonly Location Wroclaw = Create("Wroclaw Office", "Poland", "Wroclaw", "Swidnicka", "40", 50068, "Europe/Warsaw", WroclawId);
    public static readonly Location Vienna = Create("Vienna Office", "Austria", "Vienna", "Kaerntner Strasse", "9", 1010, "Europe/Vienna", ViennaId);
    public static readonly Location Graz = Create("Graz Office", "Austria", "Graz", "Herrengasse", "16", 8010, "Europe/Vienna", GrazId);
    public static readonly Location Zurich = Create("Zurich Office", "Switzerland", "Zurich", "Bahnhofstrasse", "33", 8001, "Europe/Zurich", ZurichId);
    public static readonly Location Geneva = Create("Geneva Office", "Switzerland", "Geneva", "Rue du Rhone", "48", 1204, "Europe/Zurich", GenevaId);
    public static readonly Location Amsterdam = Create("Amsterdam Office", "Netherlands", "Amsterdam", "Damrak", "70", 1012, "Europe/Amsterdam", AmsterdamId);
    public static readonly Location Rotterdam = Create("Rotterdam Office", "Netherlands", "Rotterdam", "Coolsingel", "40", 3011, "Europe/Amsterdam", RotterdamId);
    public static readonly Location Brussels = Create("Brussels Office", "Belgium", "Brussels", "Rue Neuve", "30", 1000, "Europe/Brussels", BrusselsId);
    public static readonly Location Luxembourg = Create("Luxembourg Office", "Luxembourg", "Luxembourg", "Grand-Rue", "12", 1660, "Europe/Luxembourg", LuxembourgId);
    public static readonly Location Prague = Create("Prague Office", "Czech Republic", "Prague", "Vaclavske namesti", "11", 11000, "Europe/Prague", PragueId);
    public static readonly Location Brno = Create("Brno Office", "Czech Republic", "Brno", "Masarykova", "6", 60200, "Europe/Prague", BrnoId);
    public static readonly Location Budapest = Create("Budapest Office", "Hungary", "Budapest", "Vaci utca", "19", 1052, "Europe/Budapest", BudapestId);
    public static readonly Location Bratislava = Create("Bratislava Office", "Slovakia", "Bratislava", "Obchodna", "10", 81106, "Europe/Bratislava", BratislavaId);
    public static readonly Location Ljubljana = Create("Ljubljana Office", "Slovenia", "Ljubljana", "Copova ulica", "5", 1000, "Europe/Ljubljana", LjubljanaId);
    public static readonly Location Zagreb = Create("Zagreb Office", "Croatia", "Zagreb", "Ilica", "1", 10000, "Europe/Zagreb", ZagrebId);
    public static readonly Location Bucharest = Create("Bucharest Office", "Romania", "Bucharest", "Calea Victoriei", "25", 10063, "Europe/Bucharest", BucharestId);

    // ── Western / Southern Europe ────────────────────────────────────────
    public static readonly Location Paris = Create("Paris Office", "France", "Paris", "Rue de Rivoli", "99", 75001, "Europe/Paris", ParisId);
    public static readonly Location Lyon = Create("Lyon Office", "France", "Lyon", "Rue de la Republique", "50", 69002, "Europe/Paris", LyonId);
    public static readonly Location Madrid = Create("Madrid Office", "Spain", "Madrid", "Gran Via", "32", 28013, "Europe/Madrid", MadridId);
    public static readonly Location Barcelona = Create("Barcelona Office", "Spain", "Barcelona", "Passeig de Gracia", "21", 8007, "Europe/Madrid", BarcelonaId);
    public static readonly Location Lisbon = Create("Lisbon Office", "Portugal", "Lisbon", "Rua Augusta", "100", 1100, "Europe/Lisbon", LisbonId);
    public static readonly Location Porto = Create("Porto Office", "Portugal", "Porto", "Rua de Santa Catarina", "112", 4000, "Europe/Lisbon", PortoId);
    public static readonly Location Turin = Create("Turin Office", "Italy", "Turin", "Via Roma", "101", 10123, "Europe/Rome", TurinId);
    public static readonly Location Milan = Create("Milan Office", "Italy", "Milan", "Via Montenapoleone", "8", 20121, "Europe/Rome", MilanId);
    public static readonly Location Athens = Create("Athens Office", "Greece", "Athens", "Ermou", "42", 10563, "Europe/Athens", AthensId);

    // ── Nordics / Baltics ────────────────────────────────────────────────
    public static readonly Location Copenhagen = Create("Copenhagen Office", "Denmark", "Copenhagen", "Stroeget", "25", 1160, "Europe/Copenhagen", CopenhagenId);
    public static readonly Location Stockholm = Create("Stockholm Office", "Sweden", "Stockholm", "Drottninggatan", "53", 11121, "Europe/Stockholm", StockholmId);
    public static readonly Location Gothenburg = Create("Gothenburg Office", "Sweden", "Gothenburg", "Kungsgatan", "44", 41119, "Europe/Stockholm", GothenburgId);
    public static readonly Location Bergen = Create("Bergen Office", "Norway", "Bergen", "Torgallmenningen", "8", 5014, "Europe/Oslo", BergenId);
    public static readonly Location Tampere = Create("Tampere Office", "Finland", "Tampere", "Hameenkatu", "10", 33100, "Europe/Helsinki", TampereId);
    public static readonly Location Tallinn = Create("Tallinn Office", "Estonia", "Tallinn", "Viru", "3", 10140, "Europe/Tallinn", TallinnId);
    public static readonly Location Riga = Create("Riga Office", "Latvia", "Riga", "Brivibas iela", "31", 1010, "Europe/Riga", RigaId);
    public static readonly Location Vilnius = Create("Vilnius Office", "Lithuania", "Vilnius", "Gedimino prospektas", "9", 1103, "Europe/Vilnius", VilniusId);

    // ── Collections ──────────────────────────────────────────────────────
    public static IReadOnlyList<Location> All =>
    [
        Berlin, Munich, Hamburg, Frankfurt, Cologne,
        Warsaw, Vienna, Zurich, Amsterdam, Prague,
        Dresden, Leipzig, Stuttgart, Duesseldorf, Hanover,
        Nuremberg, Bremen, Essen, Bonn, Karlsruhe,
        Krakow, Wroclaw, Graz, Geneva, Rotterdam,
        Brno, Paris, Lyon, Madrid, Barcelona,
        Lisbon, Porto, Turin, Milan, Brussels,
        Luxembourg, Copenhagen, Stockholm, Gothenburg, Bergen,
        Tampere, Bucharest, Budapest, Bratislava, Ljubljana,
        Zagreb, Tallinn, Riga, Vilnius, Athens
    ];

    /// <summary>Локации, к которым не привязан ни один департамент.</summary>
    public static IReadOnlyList<Location> Unassigned =>
    [
        Bremen, Karlsruhe, Brno, Luxembourg, Bergen,
        Bucharest, Ljubljana, Zagreb, Riga, Athens
    ];

    private static Location Create(
        string name, string country, string city, string street, string house,
        int postalCode, string timezone, Guid id) =>
        Location.Create(
            LocationName.Convert(name),
            Address.Convert(country, city, street, house, postalCode),
            Timezone.Convert(timezone),
            id).Value;
}
