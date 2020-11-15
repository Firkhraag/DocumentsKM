using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlEmployeeRepo : IEmployeeRepo
    {
        private readonly ApplicationContext _context;

        public SqlEmployeeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAllByDepartmentId(int departmentId)
        {
            return _context.Employees.Where(e => e.Department.Id == departmentId).ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Employee> GetAllByDepartmentIdAndPositions(
            int departmentId,
            int[] posIds)
        {
            return _context.Employees.Where(e => (e.Department.Id == departmentId) &&
                (posIds.Contains(e.Position.Id))).ToList();
        }

        public IEnumerable<Employee> GetAllByDepartmentIdAndPosition(
            int departmentId,
            int posId)
        {
            return _context.Employees.Where(e => (e.Department.Id == departmentId) &&
                (e.Position.Id == posId)).ToList();
        }
    }
}
