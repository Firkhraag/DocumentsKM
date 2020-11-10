using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class MarkOperatingConditions
    {
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }
        [Key]
        public int MarkId { get; set; }

        [Required]
        public float SafetyCoeff { get; set; }

        [Required]
        [ForeignKey("EnvAggressivenessId")]
        public virtual EnvAggressiveness EnvAggressiveness { get; set; }

        [Required]
        public int Temperature { get; set; }

        [Required]
        [ForeignKey("OperatingAreaId")]
        public virtual OperatingArea OperatingArea { get; set; }

        [Required]
        [ForeignKey("GasGroupId")]
        public virtual GasGroup GasGroup { get; set; }

        [Required]
        [ForeignKey("ConstructionMaterialId")]
        public virtual ConstructionMaterial ConstructionMaterial { get; set; }

        [Required]
        [ForeignKey("PaintworkTypeId")]
        public virtual PaintworkType PaintworkType { get; set; }

        [Required]
        [ForeignKey("HighTensileBoltsTypeId")]
        public virtual HighTensileBoltsType HighTensileBoltsType { get; set; }
    }
}
