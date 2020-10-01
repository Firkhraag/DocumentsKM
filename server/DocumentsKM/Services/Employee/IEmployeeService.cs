using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IEmployeeService
    {
        // Получить всех сотрудников для согласования
        IEnumerable<Employee> GetAllApprovalByDepartmentNumber(int departmentNumber);

        // Получить сотрудника по id
        Employee GetById(int id);
    }
}
