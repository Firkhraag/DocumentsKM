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

        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department GetById(int id)
        {
            return _context.Departments.SingleOrDefault(v => v.Id == id);
        }
    }
}
