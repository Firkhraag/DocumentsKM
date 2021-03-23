using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class AdditionalWorkUpdateRequest
    {
        public Int32? EmployeeId { get; set; }
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? Valuation { get; set; }
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16? MetalOrder { get; set; }

        public AdditionalWorkUpdateRequest()
        {
            EmployeeId = null;
            Valuation = null;
            MetalOrder = null;
        }
    }
}
