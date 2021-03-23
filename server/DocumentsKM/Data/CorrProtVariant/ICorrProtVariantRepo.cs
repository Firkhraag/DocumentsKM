using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ICorrProtVariantRepo
    {
        // Получить вариант антикоррозионной защиты по id условий эксплуатации
        CorrProtVariant GetByOperatingConditionIds(
            int envAggressivenessId,
            int constructionMaterialId,
            int gasGroupId,
            int operatingAreaId);
        // Получить вариант антикоррозионной защиты по id условий эксплуатации с типом лакокрасочного покрытия
        CorrProtVariant GetByOperatingConditionIdsWithPaintwork(
            int envAggressivenessId,
            int constructionMaterialId,
            int gasGroupId,
            int operatingAreaId,
            int paintworkTypeId);
    }
}
