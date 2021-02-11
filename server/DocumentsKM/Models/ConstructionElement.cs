using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class ConstructionElement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ConstructionId")]
        public virtual Construction Construction { get; set; }

        [Required]
        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }

        [Required]
        [ForeignKey("SteelId")]
        public virtual Steel Steel { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Value should be greater than or equal to 0")]
        public float Length { get; set; }
    }
}
