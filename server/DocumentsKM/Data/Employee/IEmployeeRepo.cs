using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public interface IEmployeeRepo
    {
        Employee GetEmployeeById(ulong id);
        IEnumerable<Employee> GetAllApprovalSpecialists(ulong departmentNumber, uint minPosCode, uint maxPosCode);
    }
}
