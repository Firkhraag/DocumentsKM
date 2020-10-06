namespace DocumentsKM.Dtos
{
    public class DepartmentResponse : DepartmentBaseResponse
    {
        public EmployeeBaseResponse DepartmentHead { get; set; }
    }
}
