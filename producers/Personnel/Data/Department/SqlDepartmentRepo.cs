using System.Linq;
using Personnel.Models;

namespace Personnel.Data
{
    public class SqlDepartmentRepo : IDepartmentRepo
    {
        private readonly ApplicationContext _context;

        public SqlDepartmentRepo(ApplicationContext context)
        {
            _context = context;
        }

        public Department GetById(int id)
        {
            return _context.Departments.SingleOrDefault(v => v.Id == id);
        }
    }
}
