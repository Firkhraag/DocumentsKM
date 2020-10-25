using System;

namespace DocumentsKM.Dtos
{
    public class SpecificationResponse
    {
        public int Id { get; set; }
        public byte ReleaseNumber { get; set; }
        public bool IsCurrent { get; set; }
        public string Note { get; set; }
        public DateTime Created { get; set; }
    }
}
