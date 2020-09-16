using DocumentsKM.Model;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SubnodeContext : DbContext
    {
        public SubnodeContext(DbContextOptions<SubnodeContext> opt) : base(opt) {}

        public DbSet<Subnode> Subnodes { get; set; }
    }
}
