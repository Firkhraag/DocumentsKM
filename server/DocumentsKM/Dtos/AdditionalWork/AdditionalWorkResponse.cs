using System;

namespace DocumentsKM.Dtos
{
    public class AdditionalWorkResponse
    {
        public Int32 Id { get; set; }
        public EmployeeBaseResponse Employee { get; set; }
        public Int16 Valuation { get; set; }
        public Int16 MetalOrder { get; set; }
        public float DrawingsCompleted { get; set; }
        public float DrawingsCheck { get; set; }
    }
}
