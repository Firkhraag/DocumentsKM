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
            return _context.Employees.Where(
                v => v.Department.Id == departmentId).OrderBy(
                    v => v.Position.Id).ToList();
        }

        public IEnumerable<Employee> GetAllByDepartmentIdAndPositions(
            int departmentId,
            int[] posIds)
        {
            return _context.Employees.Where(v => (v.Department.Id == departmentId) &&
                (posIds.Contains(v.Position.Id))).OrderBy(
                    v => v.Position.Id).ToList();
        }

        public IEnumerable<Employee> GetAllByDepartmentIdAndPosition(
            int departmentId,
            int posId)
        {
            return _context.Employees.Where(
                v => (v.Department.Id == departmentId) &&
                    (v.Position.Id == posId)).OrderBy(
                        v => v.Position.Id).ToList();
        }

        public Employee GetById(int id)
        {
            return _context.Employees.SingleOrDefault(v => v.Id == id);
        }
    }
}
