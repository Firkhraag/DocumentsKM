using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlDepartmentRepo : IDepartmentRepo
    {
        private readonly ApplicationContext _context;

        public SqlDepartmentRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Department> GetAllActive()
        {
            return _context.Departments.Where(d => d.IsActive);
        }

        public Department GetByNumber(int number)
        {
            return _context.Departments.FirstOrDefault(d => d.Number == number);
        }
    }
}
