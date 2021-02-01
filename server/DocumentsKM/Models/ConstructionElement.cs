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
        [MaxLength(30)]
        public string ProfileName { get; set; }

        [Required]
        [MaxLength(2)]
        public string Symbol { get; set; }

        [Required]
        public float Weight { get; set; }

        [Required]
        public float SurfaceArea { get; set; }

        [Required]
        [ForeignKey("ProfileTypeId")]
        public virtual ProfileType ProfileType { get; set; }

        [Required]
        [ForeignKey("SteelId")]
        public virtual Steel Steel { get; set; }

        [Required]
        public float Length { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
