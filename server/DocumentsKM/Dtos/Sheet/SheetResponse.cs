namespace DocumentsKM.Dtos
{
    public class SheetResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public float Format { get; set; }
        public EmployeeBaseResponse Creator { get; set; }
        public EmployeeBaseResponse Inspector { get; set; }
        public EmployeeBaseResponse NormController { get; set; }
        public string Note { get; set; }
    }
}
