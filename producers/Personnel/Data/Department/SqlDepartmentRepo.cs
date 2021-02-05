using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department GetById(int id)
        {
            return _context.Departments.SingleOrDefault(v => v.Id == id);
        }

        public void Add(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }

        public void Update(Department department)
        {
            _context.Entry(department).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Department department)
        {
            _context.Departments.Remove(department);
            _context.SaveChanges();
        }
    }
}
