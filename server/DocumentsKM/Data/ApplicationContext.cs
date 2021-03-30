using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> opt) : base(opt) {}

        // Other services data
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<User> Users { get; set; }

        // Current service data
        public DbSet<Mark> Marks { get; set; }
        public DbSet<MarkApproval> MarkApprovals { get; set; }

        public DbSet<ConstructionType> ConstructionTypes { get; set; }
        public DbSet<ConstructionSubtype> ConstructionSubtypes { get; set; }
        public DbSet<WeldingControl> WeldingControl { get; set; }

        public DbSet<ProfileType> ProfileTypes { get; set; }
        public DbSet<ProfileClass> ProfileClasses { get; set; }
        public DbSet<Steel> Steel { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Doc> Docs { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<Construction> Constructions { get; set; }
        public DbSet<StandardConstructionName> StandardConstructionNames { get; set; }
        public DbSet<StandardConstruction> StandardConstructions { get; set; }
        public DbSet<BoltDiameter> BoltDiameters { get; set; }
        public DbSet<BoltLength> BoltLengths { get; set; }
        public DbSet<ConstructionBolt> ConstructionBolts { get; set; }
        public DbSet<ConstructionElement> ConstructionElements { get; set; }

        public DbSet<PaintworkFastness> PaintworkFastness { get; set; }
        public DbSet<Primer> Primer { get; set; }
        public DbSet<CorrProtCleaningDegree> CorrProtCleaningDegrees { get; set; }
        public DbSet<CorrProtVariant> CorrProtVariants { get; set; }
        public DbSet<CorrProtMethod> CorrProtMethods { get; set; }
        public DbSet<CorrProtCoating> CorrProtCoatings { get; set; }
        public DbSet<DocType> DocTypes { get; set; }
        public DbSet<SheetName> SheetNames { get; set; }

        public DbSet<MarkLinkedDoc> MarkLinkedDocs { get; set; }
        public DbSet<LinkedDoc> LinkedDocs { get; set; }
        public DbSet<LinkedDocType> LinkedDocTypes { get; set; }

        public DbSet<EnvAggressiveness> EnvAggressiveness { get; set; }
        public DbSet<OperatingArea> OperatingAreas { get; set; }
        public DbSet<GasGroup> GasGroups { get; set; }
        public DbSet<ConstructionMaterial> ConstructionMaterials { get; set; }
        public DbSet<PaintworkType> PaintworkTypes { get; set; }
        public DbSet<HighTensileBoltsType> HighTensileBoltsTypes { get; set; }
        public DbSet<MarkOperatingConditions> MarkOperatingConditions { get; set; }

        public DbSet<AttachedDoc> AttachedDocs { get; set; }
        public DbSet<AdditionalWork> AdditionalWork { get; set; }

        public DbSet<GeneralDataSection> GeneralDataSections { get; set; }
        public DbSet<GeneralDataPoint> GeneralDataPoints { get; set; }
        public DbSet<MarkGeneralDataSection> MarkGeneralDataSections { get; set; }
        public DbSet<MarkGeneralDataPoint> MarkGeneralDataPoints { get; set; }

        public DbSet<DefaultValues> DefaultValues { get; set; }

        public DbSet<EstimateTask> EstimateTask { get; set; }
    }
}
