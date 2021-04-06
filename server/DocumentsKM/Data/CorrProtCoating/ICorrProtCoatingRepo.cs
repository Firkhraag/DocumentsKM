using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ICorrProtCoatingRepo
    {
        // Получить покрытие антикоррозионной защиты по стойкости, типу и группе лп
        CorrProtCoating GetByFastnessTypeAndGroup(
            int paintworkFastnessId,
            int paintworkTypeId,
            int paintworkGroup);
    }
}
