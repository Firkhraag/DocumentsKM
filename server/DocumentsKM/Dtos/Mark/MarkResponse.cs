using System;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class MarkResponse
    {
        public Int32 Id { get; set; }
        public string Code { get; set; }
        public virtual Department Department { get; set; }
        public string ChiefEngineer { get; set; }
        public string Name { get; set; }
        public string ComplexName { get; set; }
        public string ObjectName { get; set; }
        public virtual EmployeeBaseResponse ChiefSpecialist { get; set; }
        public virtual EmployeeBaseResponse GroupLeader { get; set; }
        public virtual EmployeeBaseResponse NormContr { get; set; }
    }
}
