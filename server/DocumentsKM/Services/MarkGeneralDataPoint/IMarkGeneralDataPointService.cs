using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IMarkGeneralDataPointService
    {
        // Получить все пункты общих указаний по id марки и раздела
        IEnumerable<MarkGeneralDataPoint> GetAllByMarkAndSectionId(
            int markId, int sectionId);
        // Создать пункт общих указаний
        void Create(
            MarkGeneralDataPoint generalDataPoint,
            int markId,
            int sectionId);
        // Обновить существующий пункт общих указаний
        void Update(int id, int markId, int sectionId,
            MarkGeneralDataPointUpdateRequest generalDataPoint);
        // Удалить существующий пункт общих указаний
        void Delete(int id, int markId, int sectionId);
        // Получить разделы общих указаний по id марки
        IEnumerable<GeneralDataSection> GetSectionsByMarkId(int markId);
    }
}
