using DirectoryService.Domain.Departments;
using DirectoryService.Domain.ValueObjects;
using DirectoryService.TestData.Fixtures;

namespace DirectoryService.TestData.Fixtures;

public static class DepartmentFixtures
{
    public static readonly Guid ItId = Guid.Parse("00000000-0000-0000-0001-000000000001");
    public static readonly Guid SoftwareDevId = Guid.Parse("00000000-0000-0000-0001-000000000002");
    public static readonly Guid InfraDevOpsId = Guid.Parse("00000000-0000-0000-0001-000000000003");
    public static readonly Guid HrId = Guid.Parse("00000000-0000-0000-0001-000000000004");
    public static readonly Guid RecruitingId = Guid.Parse("00000000-0000-0000-0001-000000000005");
    public static readonly Guid FinanceId = Guid.Parse("00000000-0000-0000-0001-000000000006");
    public static readonly Guid AccountingId = Guid.Parse("00000000-0000-0000-0001-000000000007");
    public static readonly Guid SalesId = Guid.Parse("00000000-0000-0000-0001-000000000008");
    public static readonly Guid MarketingId = Guid.Parse("00000000-0000-0000-0001-000000000009");
    public static readonly Guid LegalId = Guid.Parse("00000000-0000-0000-0001-000000000010");

    public static readonly Department IT = Department.CreateParent(
        DepartmentName.Convert("Information Technology"),
        Identifier.Convert("IT"),
        [new DepartmentLocation(ItId, LocationFixtures.BerlinId)],
        ItId).Value;

    public static readonly Department SoftwareDev = Department.CreateChild(
        DepartmentName.Convert("Software Development"),
        Identifier.Convert("IT-DEV"),
        IT,
        [
            new DepartmentLocation(SoftwareDevId, LocationFixtures.BerlinId),
            new DepartmentLocation(SoftwareDevId, LocationFixtures.MunichId)
        ],
        SoftwareDevId).Value;

    public static readonly Department InfraDevOps = Department.CreateChild(
        DepartmentName.Convert("Infrastructure & DevOps"),
        Identifier.Convert("IT-OPS"),
        SoftwareDev,
        [new DepartmentLocation(InfraDevOpsId, LocationFixtures.BerlinId)],
        InfraDevOpsId).Value;

    public static readonly Department HR = Department.CreateParent(
        DepartmentName.Convert("Human Resources"),
        Identifier.Convert("HR"),
        [
            new DepartmentLocation(HrId, LocationFixtures.BerlinId),
            new DepartmentLocation(HrId, LocationFixtures.HamburgId)
        ],
        HrId).Value;

    public static readonly Department Recruiting = Department.CreateChild(
        DepartmentName.Convert("Recruiting"),
        Identifier.Convert("HR-REC"),
        HR,
        [new DepartmentLocation(RecruitingId, LocationFixtures.BerlinId)],
        RecruitingId).Value;

    public static readonly Department Finance = Department.CreateParent(
        DepartmentName.Convert("Finance"),
        Identifier.Convert("FIN"),
        [new DepartmentLocation(FinanceId, LocationFixtures.FrankfurtId)],
        FinanceId).Value;

    public static readonly Department Accounting = Department.CreateChild(
        DepartmentName.Convert("Accounting"),
        Identifier.Convert("FIN-ACC"),
        Finance,
        [new DepartmentLocation(AccountingId, LocationFixtures.FrankfurtId)],
        AccountingId).Value;

    public static readonly Department Sales = Department.CreateParent(
        DepartmentName.Convert("Sales"),
        Identifier.Convert("SALES"),
        [
            new DepartmentLocation(SalesId, LocationFixtures.MunichId),
            new DepartmentLocation(SalesId, LocationFixtures.CologneId)
        ],
        SalesId).Value;

    public static readonly Department Marketing = Department.CreateParent(
        DepartmentName.Convert("Marketing"),
        Identifier.Convert("MKT"),
        [
            new DepartmentLocation(MarketingId, LocationFixtures.BerlinId),
            new DepartmentLocation(MarketingId, LocationFixtures.AmsterdamId)
        ],
        MarketingId).Value;

    public static readonly Department Legal = Department.CreateParent(
        DepartmentName.Convert("Legal"),
        Identifier.Convert("LEG"),
        [
            new DepartmentLocation(LegalId, LocationFixtures.BerlinId),
            new DepartmentLocation(LegalId, LocationFixtures.ViennaId)
        ],
        LegalId).Value;

    public static IReadOnlyList<Department> All =>
    [
        IT, SoftwareDev, InfraDevOps,
        HR, Recruiting,
        Finance, Accounting,
        Sales, Marketing, Legal
    ];
}
