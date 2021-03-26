using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IMarkGeneralDataPointService
    {
        // Получить все пункты общих указаний марки по id раздела
        IEnumerable<MarkGeneralDataPoint> GetAllBySectionId(
            int sectionId);
        // Добавить пункт общих указаний к марке
        void Create(
            MarkGeneralDataPoint generalDataPoint,
            int sectionId);
        // Обновить пункты общих указаний марки по id шаблонных пунктов
        IEnumerable<MarkGeneralDataPoint> UpdateAllByPointIds(
            int userId, int sectionId, List<int> pointIds);
        // Обновить пункт общих указаний марки
        void Update(int id, int sectionId,
            MarkGeneralDataPointUpdateRequest generalDataPoint);
        // Удалить пункт общих указаний марки
        void Delete(int id, int sectionId);
        // Добавить шаблонные пункты общих указаний пользователя к марке
        void AddDefaultPoints(int userId, Mark mark);
    }
}
