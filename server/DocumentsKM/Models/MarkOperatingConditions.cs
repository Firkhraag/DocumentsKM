using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class MarkOperatingConditions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public float SafetyCoeff { get; set; }

        [Required]
        public virtual EnvAggressiveness EnvAggressiveness { get; set; }

        [Required]
        public int Temperature { get; set; }

        [Required]
        public virtual OperatingArea OperatingArea { get; set; }

        [Required]
        public virtual GasGroup GasGroup { get; set; }

        [Required]
        public virtual ConstructionMaterial ConstructionMaterial { get; set; }

        [Required]
        public virtual PaintworkType PaintworkType { get; set; }

        [Required]
        public virtual HighTensileBoltsType HighTensileBoltsType { get; set; }
    }
}
