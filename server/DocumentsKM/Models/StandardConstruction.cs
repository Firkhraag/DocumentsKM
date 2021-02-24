using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class StandardConstruction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("SpecificationId")]
        public virtual Specification Specification { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int Num { get; set; }

        [MaxLength(10)]
        public string Sheet { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Weight { get; set; }
    }
}
