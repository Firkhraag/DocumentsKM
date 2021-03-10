using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IMarkGeneralDataPointService
    {
        // Получить все пункты общих указаний марки по id марки и раздела
        IEnumerable<MarkGeneralDataPoint> GetAllByMarkAndSectionId(
            int markId, int sectionId);
        // Добавить пункт общих указаний к марке
        void Create(
            MarkGeneralDataPoint generalDataPoint,
            int markId,
            int sectionId);
        // Обновить пункты общих указаний марки по id разделов
        void UpdateAllBySectionIds(int markId, List<int> sectionIds);
        // Обновить пункты общих указаний марки по id шаблонных пунктов
        IEnumerable<MarkGeneralDataPoint> UpdateAllByPointIds(
            int userId, int markId, int sectionId, List<int> pointIds);
        // Обновить пункт общих указаний марки
        void Update(int id, int markId, int sectionId,
            MarkGeneralDataPointUpdateRequest generalDataPoint);
        // Удалить пункт общих указаний марки
        void Delete(int id, int markId, int sectionId);
        // Получить разделы общих указаний по id марки
        IEnumerable<GeneralDataSection> GetSectionsByMarkId(int markId);
        // Добавить пункты общих указаний пользователя к марке
        void AddDefaultPoints(int userId, Mark mark);
    }
}
