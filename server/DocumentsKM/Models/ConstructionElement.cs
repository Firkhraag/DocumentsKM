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
        [ForeignKey("ProfileClassId")]
        public virtual ProfileClass ProfileClass { get; set; }

        [Required]
        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }

        [Required]
        [ForeignKey("SteelId")]
        public virtual Steel Steel { get; set; }

        [Required]
        public float Length { get; set; }
    }
}
