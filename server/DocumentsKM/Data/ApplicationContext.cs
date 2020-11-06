using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

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
            builder.Entity<MarkApproval>().HasKey(e => new { e.MarkId, e.EmployeeId });

            // Unique constrains
            builder.Entity<User>()
                .HasIndex(e => e.Login)
                .IsUnique();
            // builder.Entity<Project>()
            //     .HasIndex(e => e.BaseSeries)
            //     .IsUnique();
            // builder.Entity<Node>()
            //     .HasIndex(e => e.Code)
            //     .IsUnique();
            // builder.Entity<Subnode>()
            //     .HasIndex(e => e.Code)
            //     .IsUnique();
            // builder.Entity<Mark>()
            //     .HasIndex(e => e.Code)
            //     .IsUnique();
            // // Composite
            // builder.Entity<Mark>().HasIndex(e => new { e.SubnodeId, e.Code }).IsUnique();
            // builder.Entity<Specification>().HasIndex(e => new { e.MarkId, e.Num }).IsUnique();
            // builder.Entity<Sheet>().HasIndex(e => new { e.MarkId, e.Num, e.DocTypeId }).IsUnique();

            // Default datetime
            // builder.Entity<Mark>().Property(e => e.EditedDate).HasDefaultValueSql("now()");
            // builder.Entity<Specification>().Property(e => e.CreatedDate).HasDefaultValueSql("now()");

            foreach(var entity in builder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToSnakeCase());

                foreach(var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());
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
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<Sheet> Sheets { get; set; }

        public DbSet<CorrProtCleaningDegree> CorrProtCleaningDegrees { get; set; }
        public DbSet<CorrProtVariant> CorrProtVariants { get; set; }
        public DbSet<MarkDesignation> MarkDesignations { get; set; }
        public DbSet<DocType> DocTypes { get; set; }
        public DbSet<SheetName> SheetNames { get; set; }

        public DbSet<MarkLinkedDoc> MarkLinkedDocs { get; set; }
        public DbSet<LinkedDoc> LinkedDocs { get; set; }
        public DbSet<LinkedDocType> LinkedDocTypes { get; set; }
    }
}
