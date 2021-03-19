using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlEmployeeRepo : IEmployeeRepo
    {
        private readonly ApplicationContext _context;

        public SqlEmployeeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public IEnumerable<Employee> GetAllActive()
        {
            return _context.Employees.Where(v => v.IsActive).OrderBy(
                v => v.Position.Id).ToList();
        }

        public IEnumerable<Employee> GetAllByDepartmentId(int departmentId)
        {
            return _context.Employees.Where(
                v => v.Department.Id == departmentId && v.IsActive).OrderBy(
                    v => v.Position.Id).ToList();
        }

        public IEnumerable<Employee> GetAllByDepartmentIdAndPositions(
            int departmentId,
            int minPosId,
            int maxPosId)
        {
            return _context.Employees.Where(v => v.Department.Id == departmentId &&
                v.Position.Id >= minPosId && v.Position.Id <= maxPosId && v.IsActive).OrderBy(
                    v => v.Position.Id).ToList();
        }

        public IEnumerable<Employee> GetAllByDepartmentIdAndPositions(
            int departmentId,
            int[] posIds)
        {
            return _context.Employees.Where(v => (v.Department.Id == departmentId) &&
                (posIds.Contains(v.Position.Id)  && v.IsActive)).OrderBy(
                    v => v.Position.Id).ToList();
        }

        public IEnumerable<Employee> GetAllByDepartmentIdAndPosition(
            int departmentId,
            int posId)
        {
            return _context.Employees.Where(
                v => v.Department.Id == departmentId &&
                    v.Position.Id == posId && v.IsActive).OrderBy(
                        v => v.Position.Id).ToList();
        }

        public Employee GetByDepartmentIdAndPosition(
            int departmentId,
            int posId)
        {
            return _context.Employees.FirstOrDefault(
                v => v.Department.Id == departmentId &&
                    v.Position.Id == posId && v.IsActive);
        }

        public Employee GetById(int id)
        {
            return _context.Employees.SingleOrDefault(v => v.Id == id);
        }

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void Update(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}
