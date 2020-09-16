using DocumentsKM.Model;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class MarkContext : DbContext
    {
        public MarkContext(DbContextOptions<MarkContext> opt) : base(opt) {}

        public DbSet<Mark> Marks { get; set; }
    }
}
