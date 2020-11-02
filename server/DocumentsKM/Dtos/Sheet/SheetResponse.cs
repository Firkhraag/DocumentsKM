namespace DocumentsKM.Dtos
{
    public class SheetResponse
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public string Name { get; set; }
        public float Form { get; set; }
        public EmployeeBaseResponse Creator { get; set; }
        public EmployeeBaseResponse Inspector { get; set; }
        public EmployeeBaseResponse NormContr { get; set; }
        public string Note { get; set; }
    }
}
