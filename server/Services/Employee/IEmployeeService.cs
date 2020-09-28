using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IEmployeeService
    {
        // Получить всех сотрудников для согласования
        IEnumerable<Employee> GetAllApprovalByDepartmentId(int departmentId);
    }
}
