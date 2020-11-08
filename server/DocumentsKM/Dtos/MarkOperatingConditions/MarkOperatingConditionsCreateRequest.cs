using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkOperatingConditionsCreateRequest
    {
        [Required]
        public float SafetyCoeff { get; set; }

        [Required]
        public int EnvAggressivenessId { get; set; }

        [Required]
        public int Temperature { get; set; }

        [Required]
        public int OperatingAreaId { get; set; }

        [Required]
        public int GasGroupId { get; set; }

        [Required]
        public int ConstructionMaterialId { get; set; }

        [Required]
        public int PaintworkTypeId { get; set; }

        [Required]
        public int HighTensileBoltsTypeId { get; set; }
    }
}
