namespace DocumentsKM.Dtos
{
    public class MarkOperatingConditionsUpdateRequest
    {
        public float? SafetyCoeff { get; set; }
        public int? EnvAggressivenessId { get; set; }
        public int? Temperature { get; set; }
        public int? OperatingAreaId { get; set; }
        public int? GasGroupId { get; set; }
        public int? ConstructionMaterialId { get; set; }
        public int? PaintworkTypeId { get; set; }
        public int? HighTensileBoltsTypeId { get; set; }
    }
}
