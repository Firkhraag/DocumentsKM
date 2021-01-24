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
        public int Valuation { get; set; }

        [Required]
        public int MetalOrder { get; set; }
    }
}
