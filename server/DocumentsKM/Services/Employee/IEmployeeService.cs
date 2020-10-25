using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IEmployeeService
    {
        // Получить всех сотрудников для согласования
        IEnumerable<Employee> GetMarkApprovalEmployeesByDepartmentNumber(int departmentNumber);
        // Получить всех главных сотрудников для марки
        (Employee, IEnumerable<Employee>, IEnumerable<Employee>, IEnumerable<Employee>) GetMarkMainEmployeesByDepartmentNumber(
            int departmentNumber);
    }
}
