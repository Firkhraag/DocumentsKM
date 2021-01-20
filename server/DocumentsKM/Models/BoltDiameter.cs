using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class BoltDiameter
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Diameter { get; set; }

        [Required]
        public float NutWeight { get; set; }

        [Required]
        [MaxLength(50)]
        public string WasherSteel { get; set; }

        [Required]
        public float WasherWeight { get; set; }

        [Required]
        public int WasherThickness { get; set; }

        [Required]
        [MaxLength(50)]
        public string BoltTechSpec { get; set; }

        [Required]
        [MaxLength(50)]
        public string StrengthClass { get; set; }

        [Required]
        [MaxLength(50)]
        public string NutTechSpec { get; set; }

        [Required]
        [MaxLength(50)]
        public string WasherTechSpec { get; set; }
    }
}
