namespace DocumentsKM.Dtos
{
    public class AddWorkDocResponse
    {
        public float Form { get; set; }
        public EmployeeBaseResponse Creator { get; set; }
        public EmployeeBaseResponse NormContr { get; set; }
        public int numOfPages { get; set; }
    }
}
