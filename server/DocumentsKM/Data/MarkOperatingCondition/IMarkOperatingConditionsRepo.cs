using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkOperatingConditionsRepo
    {
        // Получить технические условия марки
        MarkOperatingConditions GetByMarkId(int markId);
        // Добавить технические условия марки
        void Add(MarkOperatingConditions markOperatingConditions);
        // Обновить технические условия марки
        void Update(MarkOperatingConditions markOperatingConditions);
    }
}
