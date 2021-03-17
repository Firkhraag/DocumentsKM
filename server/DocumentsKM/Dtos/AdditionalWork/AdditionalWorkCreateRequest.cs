using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class AdditionalWorkCreateRequest
    {
        [Required]
        public Int32 EmployeeId { get; set; }

        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Valuation { get; set; }

        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 MetalOrder { get; set; }
    }
}
