using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class AdditionalWorkCreateRequest
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int Valuation { get; set; }

        [Required]
        public int MetalOrder { get; set; }
    }
}
