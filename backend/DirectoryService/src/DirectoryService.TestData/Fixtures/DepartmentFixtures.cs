using DirectoryService.Domain.Departments;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.TestData.Fixtures;

/// <summary>
/// Дерево департаментов (10 корней, максимальная глубина — 6):
///
/// IT
/// ├── Software Development
/// │   ├── Infrastructure &amp; DevOps
/// │   │   └── Cloud Platform
/// │   │       └── Kubernetes
/// │   │           └── Cluster Reliability          (глубина 6)
/// │   ├── Backend / Frontend / Mobile Development
/// │   └── Quality Assurance
/// │       └── Test Automation
/// ├── Information Security ── Security Operations Center
/// ├── IT Support ── Helpdesk
/// └── Data &amp; Analytics ── Data Engineering, Business Intelligence
/// HR ── Recruiting, Learning &amp; Development, Compensation &amp; Benefits, Employee Relations
/// Finance ── Accounting (── AP, AR), Controlling, Treasury, Tax
/// Sales ── Enterprise, SMB, Sales Operations, Partner Channel
/// Marketing ── Content, Performance, Brand
/// Legal ── Compliance, Data Protection
/// Customer Support ── Customer Success
/// Operations ── Facility Management, Procurement
/// Product Management ── Product Design
/// Research &amp; Development                          (корень без детей).
/// </summary>
public static class DepartmentFixtures
{
    // ── IDs ──────────────────────────────────────────────────────────────
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
    public static readonly Guid CloudPlatformId = Guid.Parse("00000000-0000-0000-0001-000000000011");
    public static readonly Guid KubernetesId = Guid.Parse("00000000-0000-0000-0001-000000000012");
    public static readonly Guid ClusterReliabilityId = Guid.Parse("00000000-0000-0000-0001-000000000013");
    public static readonly Guid BackendDevId = Guid.Parse("00000000-0000-0000-0001-000000000014");
    public static readonly Guid FrontendDevId = Guid.Parse("00000000-0000-0000-0001-000000000015");
    public static readonly Guid MobileDevId = Guid.Parse("00000000-0000-0000-0001-000000000016");
    public static readonly Guid QualityAssuranceId = Guid.Parse("00000000-0000-0000-0001-000000000017");
    public static readonly Guid TestAutomationId = Guid.Parse("00000000-0000-0000-0001-000000000018");
    public static readonly Guid InfoSecId = Guid.Parse("00000000-0000-0000-0001-000000000019");
    public static readonly Guid SecurityOpsCenterId = Guid.Parse("00000000-0000-0000-0001-000000000020");
    public static readonly Guid ItSupportId = Guid.Parse("00000000-0000-0000-0001-000000000021");
    public static readonly Guid HelpdeskId = Guid.Parse("00000000-0000-0000-0001-000000000022");
    public static readonly Guid DataAnalyticsId = Guid.Parse("00000000-0000-0000-0001-000000000023");
    public static readonly Guid DataEngineeringId = Guid.Parse("00000000-0000-0000-0001-000000000024");
    public static readonly Guid BusinessIntelligenceId = Guid.Parse("00000000-0000-0000-0001-000000000025");
    public static readonly Guid LearningDevelopmentId = Guid.Parse("00000000-0000-0000-0001-000000000026");
    public static readonly Guid CompensationBenefitsId = Guid.Parse("00000000-0000-0000-0001-000000000027");
    public static readonly Guid EmployeeRelationsId = Guid.Parse("00000000-0000-0000-0001-000000000028");
    public static readonly Guid AccountsPayableId = Guid.Parse("00000000-0000-0000-0001-000000000029");
    public static readonly Guid AccountsReceivableId = Guid.Parse("00000000-0000-0000-0001-000000000030");
    public static readonly Guid ControllingId = Guid.Parse("00000000-0000-0000-0001-000000000031");
    public static readonly Guid TreasuryId = Guid.Parse("00000000-0000-0000-0001-000000000032");
    public static readonly Guid TaxId = Guid.Parse("00000000-0000-0000-0001-000000000033");
    public static readonly Guid EnterpriseSalesId = Guid.Parse("00000000-0000-0000-0001-000000000034");
    public static readonly Guid SmbSalesId = Guid.Parse("00000000-0000-0000-0001-000000000035");
    public static readonly Guid SalesOperationsId = Guid.Parse("00000000-0000-0000-0001-000000000036");
    public static readonly Guid PartnerChannelId = Guid.Parse("00000000-0000-0000-0001-000000000037");
    public static readonly Guid ContentMarketingId = Guid.Parse("00000000-0000-0000-0001-000000000038");
    public static readonly Guid PerformanceMarketingId = Guid.Parse("00000000-0000-0000-0001-000000000039");
    public static readonly Guid BrandId = Guid.Parse("00000000-0000-0000-0001-000000000040");
    public static readonly Guid ComplianceId = Guid.Parse("00000000-0000-0000-0001-000000000041");
    public static readonly Guid DataProtectionId = Guid.Parse("00000000-0000-0000-0001-000000000042");
    public static readonly Guid CustomerSupportId = Guid.Parse("00000000-0000-0000-0001-000000000043");
    public static readonly Guid CustomerSuccessId = Guid.Parse("00000000-0000-0000-0001-000000000044");
    public static readonly Guid OperationsId = Guid.Parse("00000000-0000-0000-0001-000000000045");
    public static readonly Guid FacilityManagementId = Guid.Parse("00000000-0000-0000-0001-000000000046");
    public static readonly Guid ProcurementId = Guid.Parse("00000000-0000-0000-0001-000000000047");
    public static readonly Guid ProductManagementId = Guid.Parse("00000000-0000-0000-0001-000000000048");
    public static readonly Guid ProductDesignId = Guid.Parse("00000000-0000-0000-0001-000000000049");
    public static readonly Guid ResearchDevelopmentId = Guid.Parse("00000000-0000-0000-0001-000000000050");

    // Порядок объявления важен: родитель должен быть объявлен раньше ребёнка
    // (static readonly инициализируются сверху вниз).

    // ── IT ───────────────────────────────────────────────────────────────
    public static readonly Department IT = Parent("Information Technology", "IT", ItId,
        LocationFixtures.BerlinId);

    public static readonly Department SoftwareDev = Child("Software Development", "IT-DEV", IT, SoftwareDevId,
        LocationFixtures.BerlinId, LocationFixtures.MunichId);

    public static readonly Department InfraDevOps = Child("Infrastructure & DevOps", "IT-OPS", SoftwareDev, InfraDevOpsId,
        LocationFixtures.BerlinId);

    public static readonly Department CloudPlatform = Child("Cloud Platform", "IT-OPS-CLD", InfraDevOps, CloudPlatformId,
        LocationFixtures.BerlinId, LocationFixtures.DresdenId);

    public static readonly Department Kubernetes = Child("Kubernetes", "IT-OPS-KUBE", CloudPlatform, KubernetesId,
        LocationFixtures.DresdenId);

    public static readonly Department ClusterReliability = Child("Cluster Reliability", "IT-OPS-REL", Kubernetes, ClusterReliabilityId,
        LocationFixtures.DresdenId, LocationFixtures.LeipzigId);

    public static readonly Department BackendDev = Child("Backend Development", "IT-DEV-BE", SoftwareDev, BackendDevId,
        LocationFixtures.BerlinId, LocationFixtures.KrakowId, LocationFixtures.WroclawId);

    public static readonly Department FrontendDev = Child("Frontend Development", "IT-DEV-FE", SoftwareDev, FrontendDevId,
        LocationFixtures.MunichId, LocationFixtures.PragueId);

    public static readonly Department MobileDev = Child("Mobile Development", "IT-DEV-MOB", SoftwareDev, MobileDevId,
        LocationFixtures.WarsawId);

    public static readonly Department QualityAssurance = Child("Quality Assurance", "IT-DEV-QA", SoftwareDev, QualityAssuranceId,
        LocationFixtures.KrakowId);

    public static readonly Department TestAutomation = Child("Test Automation", "IT-DEV-QA-AUT", QualityAssurance, TestAutomationId,
        LocationFixtures.KrakowId, LocationFixtures.BudapestId);

    public static readonly Department InfoSec = Child("Information Security", "IT-SEC", IT, InfoSecId,
        LocationFixtures.BerlinId, LocationFixtures.ZurichId);

    public static readonly Department SecurityOpsCenter = Child("Security Operations Center", "IT-SEC-SOC", InfoSec, SecurityOpsCenterId,
        LocationFixtures.ZurichId, LocationFixtures.GenevaId);

    public static readonly Department ItSupport = Child("IT Support", "IT-SUP", IT, ItSupportId,
        LocationFixtures.BerlinId, LocationFixtures.HamburgId, LocationFixtures.FrankfurtId);

    public static readonly Department Helpdesk = Child("Helpdesk", "IT-SUP-HD", ItSupport, HelpdeskId,
        LocationFixtures.LisbonId, LocationFixtures.PortoId);

    public static readonly Department DataAnalytics = Child("Data & Analytics", "IT-DATA", IT, DataAnalyticsId,
        LocationFixtures.MunichId);

    public static readonly Department DataEngineering = Child("Data Engineering", "IT-DATA-ENG", DataAnalytics, DataEngineeringId,
        LocationFixtures.MunichId, LocationFixtures.StockholmId);

    public static readonly Department BusinessIntelligence = Child("Business Intelligence", "IT-DATA-BI", DataAnalytics, BusinessIntelligenceId,
        LocationFixtures.StuttgartId);

    // ── HR ───────────────────────────────────────────────────────────────
    public static readonly Department HR = Parent("Human Resources", "HR", HrId,
        LocationFixtures.BerlinId, LocationFixtures.HamburgId);

    public static readonly Department Recruiting = Child("Recruiting", "HR-REC", HR, RecruitingId,
        LocationFixtures.BerlinId);

    public static readonly Department LearningDevelopment = Child("Learning & Development", "HR-LND", HR, LearningDevelopmentId,
        LocationFixtures.HamburgId, LocationFixtures.HanoverId);

    public static readonly Department CompensationBenefits = Child("Compensation & Benefits", "HR-CNB", HR, CompensationBenefitsId,
        LocationFixtures.HamburgId);

    public static readonly Department EmployeeRelations = Child("Employee Relations", "HR-ER", HR, EmployeeRelationsId,
        LocationFixtures.BonnId);

    // ── Finance ──────────────────────────────────────────────────────────
    public static readonly Department Finance = Parent("Finance", "FIN", FinanceId,
        LocationFixtures.FrankfurtId);

    public static readonly Department Accounting = Child("Accounting", "FIN-ACC", Finance, AccountingId,
        LocationFixtures.FrankfurtId);

    public static readonly Department AccountsPayable = Child("Accounts Payable", "FIN-ACC-AP", Accounting, AccountsPayableId,
        LocationFixtures.FrankfurtId, LocationFixtures.BratislavaId);

    public static readonly Department AccountsReceivable = Child("Accounts Receivable", "FIN-ACC-AR", Accounting, AccountsReceivableId,
        LocationFixtures.FrankfurtId, LocationFixtures.BratislavaId);

    public static readonly Department Controlling = Child("Controlling", "FIN-CTRL", Finance, ControllingId,
        LocationFixtures.FrankfurtId, LocationFixtures.DuesseldorfId);

    public static readonly Department Treasury = Child("Treasury", "FIN-TRS", Finance, TreasuryId,
        LocationFixtures.FrankfurtId, LocationFixtures.BrusselsId);

    public static readonly Department Tax = Child("Tax", "FIN-TAX", Finance, TaxId,
        LocationFixtures.DuesseldorfId);

    // ── Sales ────────────────────────────────────────────────────────────
    public static readonly Department Sales = Parent("Sales", "SALES", SalesId,
        LocationFixtures.MunichId, LocationFixtures.CologneId);

    public static readonly Department EnterpriseSales = Child("Enterprise Sales", "SALES-ENT", Sales, EnterpriseSalesId,
        LocationFixtures.MunichId, LocationFixtures.ParisId, LocationFixtures.MilanId);

    public static readonly Department SmbSales = Child("SMB Sales", "SALES-SMB", Sales, SmbSalesId,
        LocationFixtures.CologneId, LocationFixtures.EssenId, LocationFixtures.NurembergId);

    public static readonly Department SalesOperations = Child("Sales Operations", "SALES-OPS", Sales, SalesOperationsId,
        LocationFixtures.CologneId);

    public static readonly Department PartnerChannel = Child("Partner Channel", "SALES-PRT", Sales, PartnerChannelId,
        LocationFixtures.ViennaId, LocationFixtures.GrazId, LocationFixtures.TallinnId, LocationFixtures.VilniusId);

    // ── Marketing ────────────────────────────────────────────────────────
    public static readonly Department Marketing = Parent("Marketing", "MKT", MarketingId,
        LocationFixtures.BerlinId, LocationFixtures.AmsterdamId);

    public static readonly Department ContentMarketing = Child("Content Marketing", "MKT-CNT", Marketing, ContentMarketingId,
        LocationFixtures.AmsterdamId, LocationFixtures.RotterdamId);

    public static readonly Department PerformanceMarketing = Child("Performance Marketing", "MKT-PERF", Marketing, PerformanceMarketingId,
        LocationFixtures.AmsterdamId, LocationFixtures.BarcelonaId);

    public static readonly Department Brand = Child("Brand", "MKT-BRD", Marketing, BrandId,
        LocationFixtures.ParisId, LocationFixtures.LyonId);

    // ── Legal ────────────────────────────────────────────────────────────
    public static readonly Department Legal = Parent("Legal", "LEG", LegalId,
        LocationFixtures.BerlinId, LocationFixtures.ViennaId);

    public static readonly Department Compliance = Child("Compliance", "LEG-CMP", Legal, ComplianceId,
        LocationFixtures.ViennaId, LocationFixtures.BrusselsId);

    public static readonly Department DataProtection = Child("Data Protection", "LEG-DP", Legal, DataProtectionId,
        LocationFixtures.BerlinId);

    // ── Customer Support ─────────────────────────────────────────────────
    public static readonly Department CustomerSupport = Parent("Customer Support", "CS", CustomerSupportId,
        LocationFixtures.LisbonId, LocationFixtures.MadridId, LocationFixtures.TurinId);

    public static readonly Department CustomerSuccess = Child("Customer Success", "CS-SUC", CustomerSupport, CustomerSuccessId,
        LocationFixtures.MadridId, LocationFixtures.CopenhagenId);

    // ── Operations ───────────────────────────────────────────────────────
    public static readonly Department Operations = Parent("Operations", "OPS", OperationsId,
        LocationFixtures.BerlinId);

    public static readonly Department FacilityManagement = Child("Facility Management", "OPS-FAC", Operations, FacilityManagementId,
        LocationFixtures.BerlinId, LocationFixtures.HamburgId, LocationFixtures.MunichId);

    public static readonly Department Procurement = Child("Procurement", "OPS-PRC", Operations, ProcurementId,
        LocationFixtures.GothenburgId);

    // ── Product ──────────────────────────────────────────────────────────
    public static readonly Department ProductManagement = Parent("Product Management", "PM", ProductManagementId,
        LocationFixtures.BerlinId, LocationFixtures.StockholmId);

    public static readonly Department ProductDesign = Child("Product Design", "PM-DES", ProductManagement, ProductDesignId,
        LocationFixtures.CopenhagenId, LocationFixtures.TampereId);

    // ── R&D (корень без детей) ───────────────────────────────────────────
    public static readonly Department ResearchDevelopment = Parent("Research & Development", "RND", ResearchDevelopmentId,
        LocationFixtures.MunichId, LocationFixtures.ZurichId);

    // ── Collections ──────────────────────────────────────────────────────

    /// <summary>Все департаменты; родители всегда идут раньше детей — удобно для сидинга.</summary>
    public static IReadOnlyList<Department> All =>
    [
        IT, SoftwareDev, InfraDevOps, CloudPlatform, Kubernetes, ClusterReliability,
        BackendDev, FrontendDev, MobileDev, QualityAssurance, TestAutomation,
        InfoSec, SecurityOpsCenter, ItSupport, Helpdesk,
        DataAnalytics, DataEngineering, BusinessIntelligence,
        HR, Recruiting, LearningDevelopment, CompensationBenefits, EmployeeRelations,
        Finance, Accounting, AccountsPayable, AccountsReceivable, Controlling, Treasury, Tax,
        Sales, EnterpriseSales, SmbSales, SalesOperations, PartnerChannel,
        Marketing, ContentMarketing, PerformanceMarketing, Brand,
        Legal, Compliance, DataProtection,
        CustomerSupport, CustomerSuccess,
        Operations, FacilityManagement, Procurement,
        ProductManagement, ProductDesign,
        ResearchDevelopment
    ];

    public static IReadOnlyList<Department> Roots =>
    [
        IT, HR, Finance, Sales, Marketing, Legal,
        CustomerSupport, Operations, ProductManagement, ResearchDevelopment
    ];

    private static Department Parent(string name, string identifier, Guid id, params Guid[] locationIds) =>
        Department.CreateParent(
            DepartmentName.Convert(name),
            Identifier.Convert(identifier),
            ToLocations(id, locationIds),
            id).Value;

    private static Department Child(string name, string identifier, Department parent, Guid id, params Guid[] locationIds) =>
        Department.CreateChild(
            DepartmentName.Convert(name),
            Identifier.Convert(identifier),
            parent,
            ToLocations(id, locationIds),
            id).Value;

    private static List<DepartmentLocation> ToLocations(Guid departmentId, Guid[] locationIds) =>
        locationIds.Select(locationId => new DepartmentLocation(departmentId, locationId)).ToList();
}
