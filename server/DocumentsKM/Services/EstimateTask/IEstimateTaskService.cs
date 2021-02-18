using DocumentsKM.Models;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public interface IEstimateTaskService
    {
        // Получить данные задания на смету по id марки
        EstimateTask GetByMarkId(int markId);
        // Добавить данные задания на смету
        void Create(EstimateTask estimateTask,
            int markId,
            int? approvalEmployeeId);
        // Изменить данные задания на смету
        void Update(int markId, EstimateTaskUpdateRequest estimateTask);
        
    }
}
