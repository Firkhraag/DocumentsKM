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

            // Composite primary key
            // builder.Entity<EntityName>().HasKey(e => new { e.MarkId, e.EmployeeId });

            // Unique constrains
            builder.Entity<User>()
                .HasIndex(e => e.Login)
                .IsUnique();
            // Composite
            builder.Entity<Mark>().HasIndex(e => new { e.SubnodeId, e.Code }).IsUnique();
            builder.Entity<Specification>().HasIndex(e => new { e.MarkId, e.ReleaseNumber }).IsUnique();

            // Default datetime
            builder.Entity<Node>().Property(e => e.Created).HasDefaultValueSql("now()");
            builder.Entity<Subnode>().Property(e => e.Created).HasDefaultValueSql("now()");
            builder.Entity<Specification>().Property(e => e.Created).HasDefaultValueSql("now()");

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

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Subnode> Subnodes { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
