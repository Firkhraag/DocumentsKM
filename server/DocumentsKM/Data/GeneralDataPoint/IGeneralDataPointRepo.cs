using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IGeneralDataPointRepo
    {
        // Получить все пункты общих указаний по id пользователя и раздела
        IEnumerable<GeneralDataPoint> GetAllByUserAndSectionId(
            int userId, int sectionId);
        // Получить все пункты общих указаний по id пользователя
        IEnumerable<GeneralDataPoint> GetAllByUserId(int userId);
        // Получить пункт общих указаний по id
        GeneralDataPoint GetById(int id);
        // Получить пункт общих указаний по unique key
        GeneralDataPoint GetByUniqueKey(
            int userId, int sectionId, string text);
        // Добавить пункт общих указаний
        void Add(GeneralDataPoint generalDataPoint);
        // Обновить пункт общих указаний
        void Update(GeneralDataPoint generalDataPoint);
        // Удалить пункт общих указаний
        void Delete(GeneralDataPoint generalDataPoint);
    }
}
