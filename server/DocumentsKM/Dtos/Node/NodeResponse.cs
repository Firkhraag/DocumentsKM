using System;

namespace DocumentsKM.Dtos
{
    public class NodeResponse
    {
        public Int16 Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public EmployeeBaseResponse ChiefEngineer { get; set; }
    }
}
