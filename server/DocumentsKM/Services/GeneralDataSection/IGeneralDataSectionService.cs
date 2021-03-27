using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IGeneralDataSectionService
    {
        // Получить все раздел общих указаний марки по id марки
        IEnumerable<GeneralDataSection> GetAllByUserId(int userId);
        // Добавить раздел общих указаний к марке
        void Create(GeneralDataSection generalDataSection, int Id);
        // Обновить раздел общих указаний марки
        void Update(int id, int userId,
            GeneralDataSectionUpdateRequest generalDataSection);
        // Удалить раздел общих указаний марки
        void Delete(int id, int userId);
        // Скопировать раздел с пунктами у марки
        void Copy(int userId, GeneralDataSectionCopyRequest generalDataSectionRequest);
    }
}
