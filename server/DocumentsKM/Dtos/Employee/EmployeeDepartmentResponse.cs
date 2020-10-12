namespace DocumentsKM.Dtos
{
    public class EmployeeDepartmentResponse : EmployeeBaseResponse
    {
        public DepartmentBaseResponse Department { get; set; }
    }
}
