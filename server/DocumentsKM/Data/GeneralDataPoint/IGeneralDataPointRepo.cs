using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IGeneralDataPointRepo
    {
        // Получить все пункты общих указаний по id раздела
        IEnumerable<GeneralDataPoint> GetAllBySectionId(
            int sectionId);
        // Получить все пункты общих указаний по id пользователя
        IEnumerable<GeneralDataPoint> GetAllByUserId(
            int userId);
        // Получить пункт общих указаний по id
        GeneralDataPoint GetById(int id);
        // Получить пункт общих указаний по unique key
        GeneralDataPoint GetByUniqueKey(
            int sectionId, string text);
        // Добавить пункт общих указаний
        void Add(GeneralDataPoint generalDataPoint);
        // Обновить пункт общих указаний
        void Update(GeneralDataPoint generalDataPoint);
        // Удалить пункт общих указаний
        void Delete(GeneralDataPoint generalDataPoint);
    }
}
