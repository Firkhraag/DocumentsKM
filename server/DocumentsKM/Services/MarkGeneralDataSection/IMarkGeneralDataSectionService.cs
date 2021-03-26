using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IMarkGeneralDataSectionService
    {
        // Получить все раздел общих указаний марки по id марки
        IEnumerable<MarkGeneralDataSection> GetAllByMarkId(int markId);
        // Добавить раздел общих указаний к марке
        void Create(MarkGeneralDataSection generalDataSection, int markId);
        // Обновить раздел общих указаний марки
        void Update(int id, int markId,
            MarkGeneralDataSectionUpdateRequest generalDataSection);
        // Обновить разделы общих указаний марки по id шаблонных разделов
        IEnumerable<MarkGeneralDataSection> UpdateAllBySectionIds(int userId, int markId, List<int> sectionIds);
        // Удалить раздел общих указаний марки
        void Delete(int id, int markId);
    }
}
