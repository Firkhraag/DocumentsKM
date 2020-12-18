using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class OperatingConditionsDataResponse
    {
        public IEnumerable<EnvAggressiveness> EnvAggressiveness { get; set; }
        public IEnumerable<OperatingArea> OperatingAreas { get; set; }
        public IEnumerable<GasGroup> GasGroups { get; set; }
        public IEnumerable<ConstructionMaterial> ConstructionMaterials { get; set; }
        public IEnumerable<PaintworkType> PaintworkTypes { get; set; }
        public IEnumerable<HighTensileBoltsType> HighTensileBoltsTypes { get; set; }

        public OperatingConditionsDataResponse(IEnumerable<EnvAggressiveness> envAggressiveness,
            IEnumerable<OperatingArea> operatingAreas,
            IEnumerable<GasGroup> gasGroups,
            IEnumerable<ConstructionMaterial> constructionMaterials,
            IEnumerable<PaintworkType> paintworkTypes,
            IEnumerable<HighTensileBoltsType> highTensileBoltsTypes)
        {
            EnvAggressiveness = envAggressiveness;
            OperatingAreas = operatingAreas;
            GasGroups = gasGroups;
            ConstructionMaterials = constructionMaterials;
            PaintworkTypes = paintworkTypes;
            HighTensileBoltsTypes = highTensileBoltsTypes;
        }
    }
}
