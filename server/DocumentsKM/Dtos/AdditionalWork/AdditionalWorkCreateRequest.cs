using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class AdditionalWorkCreateRequest
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int Valuation { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int MetalOrder { get; set; }
    }
}
