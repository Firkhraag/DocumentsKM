using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IEstimateTaskRepo
    {
        // Получить данные задания на смету по id марки
        EstimateTask GetByMarkId(int markId);
        // Добавить данные задания на смету
        void Add(EstimateTask estimateTask);
        // Изменить данные задания на смету
        void Update(EstimateTask estimateTask);
    }
}
