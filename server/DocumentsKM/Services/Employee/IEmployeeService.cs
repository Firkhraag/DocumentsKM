using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IEmployeeService
    {
        // Получить всех сотрудников по номеру отдела
        IEnumerable<Employee> GetAllByDepartmentId(int departmentId);
        // Получить всех сотрудников для согласования
        IEnumerable<Employee> GetMarkApprovalEmployeesByDepartmentId(int departmentId);
        // Получить всех главных сотрудников для марки
        (Employee, IEnumerable<Employee>, IEnumerable<Employee>, IEnumerable<Employee>) GetMarkMainEmployeesByDepartmentId(
            int departmentId);
        // Обновить сотрудников
        void UpdateAll(List<EmployeeFetched> employeesFetched);
    }
}
