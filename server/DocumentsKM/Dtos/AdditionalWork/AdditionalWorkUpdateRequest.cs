using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class AdditionalWorkUpdateRequest
    {
        public int? EmployeeId { get; set; }
        [Range(0, 10000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int? Valuation { get; set; }
        [Range(0, 10000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int? MetalOrder { get; set; }

        public AdditionalWorkUpdateRequest()
        {
            EmployeeId = null;
            Valuation = null;
            MetalOrder = null;
        }
    }
}
