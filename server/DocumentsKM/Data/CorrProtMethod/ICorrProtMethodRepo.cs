using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ICorrProtMethodRepo
    {
        // Получить способ антикоррозионной защиты по id условий эксплуатации
        CorrProtMethod GetByAggressivenessAndMaterialId(
            int envAggressivenessId,
            int constructionMaterialId);
    }
}
