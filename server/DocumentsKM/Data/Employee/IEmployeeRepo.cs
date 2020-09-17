using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public interface IEmployeeRepo
    {
        // IEnumerable<Employee> GetAllEmployees(ulong projectId);
        Employee GetEmployeeById(ulong id);
    }
}
