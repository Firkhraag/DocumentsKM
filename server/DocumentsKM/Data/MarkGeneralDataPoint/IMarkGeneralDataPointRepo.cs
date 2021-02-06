using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkGeneralDataPointRepo
    {
        // Получить все пункты общих указаний марки по id марки
        IEnumerable<MarkGeneralDataPoint> GetAllByMarkId(int markId);
        // Получить все пункты общих указаний марки по id марки и раздела
        IEnumerable<MarkGeneralDataPoint> GetAllByMarkAndSectionId(
            int markId, int sectionId);
        // Получить пункт общих указаний марки по id
        MarkGeneralDataPoint GetById(int id);
        // Получить пункт общих указаний марки по unique key
        MarkGeneralDataPoint GetByUniqueKey(
            int markId, int sectionId, string text);
        // Добавить пункт общих указаний к марке
        void Add(MarkGeneralDataPoint markGeneralDataPoint);
        // Изменить пункт общих указаний у марки
        void Update(MarkGeneralDataPoint markGeneralDataPoint);
        // Удалить пункт общих указаний у марки
        void Delete(MarkGeneralDataPoint markGeneralDataPoint);
        // Получить разделы общих указаний марки по id марки
        IEnumerable<GeneralDataSection> GetSectionsByMarkId(int markId);
    }
}
