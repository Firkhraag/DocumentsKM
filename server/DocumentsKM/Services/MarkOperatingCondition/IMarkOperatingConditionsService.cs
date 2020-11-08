using DocumentsKM.Models;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public interface IMarkOperatingConditionsService
    {
        // Получить технические условия марки
        MarkOperatingConditions GetByMarkId(int markId);
        // Создать технические условия марки
        void Create(MarkOperatingConditions markOperatingConditions,
            int markId,
            int envAggressivenessId,
            int operatingAreaId,
            int gasGroupId,
            int constructionMaterialId,
            int paintworkTypeId,
            int highTensileBoltsTypeId);
        // Обновить технические условия марки
        void Update(int markId, MarkOperatingConditionsUpdateRequest markOperatingConditions);
    }
}
