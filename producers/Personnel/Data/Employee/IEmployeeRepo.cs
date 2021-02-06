using System.Collections.Generic;
using Personnel.Models;

namespace Personnel.Data
{
    public interface IEmployeeRepo
    {
        IEnumerable<Employee> GetAllByDepartmentId(int departmentId);
        Employee GetById(int id);
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
    }
}
