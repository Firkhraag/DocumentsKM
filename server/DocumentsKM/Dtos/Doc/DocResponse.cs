using System;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class DocResponse
    {
        public Int32 Id { get; set; }
        public Int16 Num { get; set; }
        public DocType Type { get; set; }
        public string Name { get; set; }
        public float Form { get; set; }
        public EmployeeBaseResponse Creator { get; set; }
        public EmployeeBaseResponse Inspector { get; set; }
        public EmployeeBaseResponse NormContr { get; set; }
        public Int16? ReleaseNum { get; set; }
        public Int16? NumOfPages { get; set; }
        public string Note { get; set; }
    }
}
