using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class NodeContext : DbContext
    {
        public NodeContext(DbContextOptions<NodeContext> opt) : base(opt) {}
        public DbSet<Node> Nodes { get; set; }
    }
}
