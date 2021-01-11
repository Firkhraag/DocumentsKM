using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IAdditionalWorkService
    {
        // Получить все дополнительные работы по id марки
        IEnumerable<AdditionalWorkResponse> GetAllByMarkId(int markId);
        // Добавить дополнительные работы
        void Create(
            AdditionalWork additionalWork,
            int markId,
            int employeeId);
        // Обновить дополнительные работы
        void Update(int id, AdditionalWorkUpdateRequest additionalWork);
        // Удалить дополнительные работы
        void Delete(int id);
    }
}
