using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkOperatingConditionsRepo
    {
        // Получить технические условия марки по id марки
        MarkOperatingConditions GetByMarkId(int markId);
        // Добавить технические условия марки
        void Add(MarkOperatingConditions markOperatingConditions);
        // Изменить технические условия марки
        void Update(MarkOperatingConditions markOperatingConditions);
    }
}
