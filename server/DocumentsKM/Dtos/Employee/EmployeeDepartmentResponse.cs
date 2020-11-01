using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class EmployeeDepartmentResponse : EmployeeBaseResponse
    {
        public Department Department { get; set; }
    }
}
