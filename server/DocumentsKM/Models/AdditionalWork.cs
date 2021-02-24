using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Учет дополнительных работ
    public class AdditionalWork
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Mark Mark { get; set; }

        [Required]
        public virtual Employee Employee { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int Valuation { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int MetalOrder { get; set; }
    }
}
