using System;

namespace DocumentsKM.Dtos
{
    public class SheetResponse
    {
        public Int32 Id { get; set; }
        public Int16 Num { get; set; }
        public string Name { get; set; }
        public float Form { get; set; }
        public EmployeeBaseResponse Creator { get; set; }
        public EmployeeBaseResponse Inspector { get; set; }
        public EmployeeBaseResponse NormContr { get; set; }
        public string Note { get; set; }
    }
}
