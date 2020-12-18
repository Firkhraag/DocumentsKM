using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class MarkOperatingConditionsResponse
    {
        public float SafetyCoeff { get; set; }
        public EnvAggressiveness EnvAggressiveness { get; set; }
        public int Temperature { get; set; }
        public OperatingArea OperatingArea { get; set; }
        public GasGroup GasGroup { get; set; }
        public ConstructionMaterial ConstructionMaterial { get; set; }
        public PaintworkType PaintworkType { get; set; }
        public HighTensileBoltsType HighTensileBoltsType { get; set; }
    }
}
