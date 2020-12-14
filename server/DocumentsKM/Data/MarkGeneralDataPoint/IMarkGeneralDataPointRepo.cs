using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkGeneralDataPointRepo
    {
        // Получить все пункты общих указаний по id раздела
        IEnumerable<MarkGeneralDataPoint> GetAllByMarkId(int markId);
        // Получить все пункты общих указаний по id раздела
        IEnumerable<MarkGeneralDataPoint> GetAllByMarkAndSectionId(
            int markId, int sectionId);
        // Получить пункт общих указаний по id
        MarkGeneralDataPoint GetById(int id);
        // Получить пункт общих указаний по id раздела и содержанию
        MarkGeneralDataPoint GetByMarkAndSectionIdAndText(
            int markId, int sectionId, string text);
        // Добавить пункт общих указаний
        void Add(MarkGeneralDataPoint markGeneralDataPoint);
        // Обновить пункт общих указаний
        void Update(MarkGeneralDataPoint markGeneralDataPoint);
        // Удалить пункт общих указаний
        void Delete(MarkGeneralDataPoint markGeneralDataPoint);
        // Получить разделы общих указаний по id марки
        IEnumerable<GeneralDataSection> GetSectionsByMarkId(int markId);
    }
}
