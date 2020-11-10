using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class DocResponse
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public DocType Type { get; set; }
        public string Name { get; set; }
        public float Form { get; set; }
        public EmployeeBaseResponse Creator { get; set; }
        public EmployeeBaseResponse Inspector { get; set; }
        public EmployeeBaseResponse NormContr { get; set; }
        public int? ReleaseNum { get; set; }
        public int? NumOfPages { get; set; }
        public string Note { get; set; }
    }
}
