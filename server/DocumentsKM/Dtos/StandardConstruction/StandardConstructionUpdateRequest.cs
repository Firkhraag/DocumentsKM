using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class StandardConstructionUpdateRequest
    {
        [MaxLength(255)]
        public string Name { get; set; }

        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? Num { get; set; }

        [MaxLength(10)]
        public string Sheet { get; set; }

        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public float? Weight { get; set; }

        public StandardConstructionUpdateRequest()
        {
            Name = null;
            Num = null;
            Sheet = null;
            Weight = null;
        }
    }
}
