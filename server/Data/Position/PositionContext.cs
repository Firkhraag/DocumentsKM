using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class PositionContext : DbContext
    {
        public PositionContext(DbContextOptions<PositionContext> opt) : base(opt) {}
        public DbSet<Position> Positions { get; set; }
    }
}
