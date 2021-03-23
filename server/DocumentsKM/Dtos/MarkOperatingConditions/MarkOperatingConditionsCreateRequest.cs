using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkOperatingConditionsCreateRequest
    {
        [Required]
        public float SafetyCoeff { get; set; }

        [Required]
        public Int16 EnvAggressivenessId { get; set; }

        [Required]
        public Int16 Temperature { get; set; }

        [Required]
        public Int16 OperatingAreaId { get; set; }

        [Required]
        public Int16 GasGroupId { get; set; }

        [Required]
        public Int16 ConstructionMaterialId { get; set; }

        [Required]
        public Int16 PaintworkTypeId { get; set; }

        [Required]
        public Int16 HighTensileBoltsTypeId { get; set; }
    }
}
