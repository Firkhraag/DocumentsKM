using System.Linq;
using Personnel.Models;

namespace Personnel.Data
{
    public class SqlPositionRepo : IPositionRepo
    {
        private readonly ApplicationContext _context;

        public SqlPositionRepo(ApplicationContext context)
        {
            _context = context;
        }

        public Position GetById(int id)
        {
            return _context.Positions.SingleOrDefault(v => v.Id == id);
        }
    }
}
