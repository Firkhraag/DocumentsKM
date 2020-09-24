using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public interface IDepartmentRepo
    {
        Department GetDepartmentByNumber(ulong number);
        IEnumerable<Department> GetAllActiveDepartments();
    }
}
