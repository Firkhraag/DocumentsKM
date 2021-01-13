using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DocumentsKM.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> opt) : base(opt) {}

        // Postgres перевод PascalCase в snake_case
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<Entity>()
            //     .Property(e => e.Field)
            //     .HasColumnType("SMALLINT");

            // Composite primary key
            builder.Entity<MarkApproval>().HasIndex(
                e => new { e.MarkId, e.EmployeeId }).IsUnique();

            // Unique constrains
            builder.Entity<User>()
                .HasIndex(e => e.Login)
                .IsUnique();
            // builder.Entity<Entity>()
            //     .HasIndex(e => e.BaseSeries)
            //     .IsUnique();
            // builder.Entity<Entity>().HasIndex(e => new { e.SubnodeId, e.Code }).IsUnique();

            // Default datetime
            // builder.Entity<Mark>().Property(e => e.EditedDate).HasDefaultValueSql("now()");

            foreach(var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToSnakeCase());

                foreach(var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName(
                        StoreObjectIdentifier.Table(entity.GetTableName(), property.DeclaringEntityType.GetSchema())
                    ).ToSnakeCase());
                }

                foreach(var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                foreach(var key in entity.GetForeignKeys())
                {
                   key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                foreach(var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
                }
            }
        }

        // Other services data
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Subnode> Subnodes { get; set; }
        public DbSet<User> Users { get; set; }

        // Current service data
        public DbSet<Mark> Marks { get; set; }
        public DbSet<MarkApproval> MarkApprovals { get; set; }

        public DbSet<ConstructionType> ConstructionTypes { get; set; }
        public DbSet<ConstructionSubtype> ConstructionSubtypes { get; set; }
        public DbSet<WeldingControl> WeldingControl { get; set; }

        public DbSet<Doc> Docs { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<Construction> Constructions { get; set; }

        public DbSet<CorrProtCleaningDegree> CorrProtCleaningDegrees { get; set; }
        public DbSet<CorrProtVariant> CorrProtVariants { get; set; }
        public DbSet<MarkDesignation> MarkDesignations { get; set; }
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
        public DbSet<MarkGeneralDataPoint> MarkGeneralDataPoints { get; set; }
    }
}
