using System;

namespace DocumentsKM.Dtos
{
    public class SpecificationResponse
    {
        public Int32 Id { get; set; }
        public Int16 Num { get; set; }
        public bool IsCurrent { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
