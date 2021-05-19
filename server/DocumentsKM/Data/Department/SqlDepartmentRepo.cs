using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<Department> GetAllActive()
        {
            return _context.Departments.Where(v => v.IsActive).ToList();
        }

        public Department GetById(int id)
        {
            return _context.Departments.SingleOrDefault(v => v.Id == id);
        }

        public Department GetByCode(string code)
        {
            return _context.Departments.SingleOrDefault(v => v.Code == code);
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
