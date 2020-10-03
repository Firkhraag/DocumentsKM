namespace DocumentsKM.Dtos
{
    public class DepartmentResponse
    {
        public int Number { get; set; }

        public string Code { get; set; }

        public EmployeeNameResponse DepartmentHead { get; set; }

        // public DepartmentResponse(int number, string code, EmployeeNameResponse departmentHead)
        // {
        //     Number = number;
        //     Code = code;
        //     DepartmentHead = departmentHead;
        // }
    }
}
