using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlCorrProtVariantRepo : ICorrProtVariantRepo
    {
        private readonly ApplicationContext _context;

        public SqlCorrProtVariantRepo(ApplicationContext context)
        {
            _context = context;
        }

        public CorrProtVariant GetByOperatingConditionIds(
            int envAggressivenessId,
            int constructionMaterialId,
            int gasGroupId,
            int operatingAreaId)
        {
            return _context.CorrProtVariants.FirstOrDefault(v =>
                v.EnvAggressiveness.Id == envAggressivenessId &&
                v.ConstructionMaterial.Id == constructionMaterialId &&
                v.GasGroup.Id == gasGroupId &&
                v.OperatingArea.Id == operatingAreaId);
        }

        public CorrProtVariant GetByOperatingConditionIdsWithPaintwork(
            int envAggressivenessId,
            int constructionMaterialId,
            int gasGroupId,
            int operatingAreaId,
            int paintworkTypeId)
        {
            return _context.CorrProtVariants.FirstOrDefault(v =>
                v.EnvAggressiveness.Id == envAggressivenessId &&
                v.ConstructionMaterial.Id == constructionMaterialId &&
                v.GasGroup.Id == gasGroupId &&
                v.OperatingArea.Id == operatingAreaId &&
                v.PaintworkType.Id == paintworkTypeId);
        }
    }
}
