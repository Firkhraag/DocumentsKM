using System.Collections.Generic;
using Personnel.Dtos;
using Personnel.Models;

namespace Personnel.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAllByDepartmentId(int departmentId);
        void Create(
            Employee employee,
            int departmentId,
            int positionId);
        void Update(int id, EmployeeRequest employee);
        void Delete(int id);
    }
}
