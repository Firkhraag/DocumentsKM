using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class ConstructionElementCreateRequest
    {
        [Required]
        public Int32 ProfileId { get; set; }

        [Required]
        public Int16 SteelId { get; set; }

        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Length { get; set; }
    }
}
