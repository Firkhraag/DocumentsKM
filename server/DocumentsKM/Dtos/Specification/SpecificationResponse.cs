using System;

namespace DocumentsKM.Dtos
{
    public class SpecificationResponse
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public bool IsCurrent { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
