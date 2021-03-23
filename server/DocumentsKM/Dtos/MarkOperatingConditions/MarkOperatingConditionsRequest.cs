using System;

namespace DocumentsKM.Dtos
{
    public class MarkOperatingConditionsUpdateRequest
    {
        public float? SafetyCoeff { get; set; }
        public Int16? EnvAggressivenessId { get; set; }
        public Int16? Temperature { get; set; }
        public Int16? OperatingAreaId { get; set; }
        public Int16? GasGroupId { get; set; }
        public Int16? ConstructionMaterialId { get; set; }
        public Int16? PaintworkTypeId { get; set; }
        public Int16? HighTensileBoltsTypeId { get; set; }
    }
}
