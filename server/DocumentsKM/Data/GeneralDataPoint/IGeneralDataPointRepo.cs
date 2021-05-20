using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IGeneralDataPointRepo
    {
        // Получить все пункты общих указаний по id раздела
        IEnumerable<GeneralDataPoint> GetAllBySectionId(
            int sectionId);
        // Получить все пункты общих указаний по названию раздела
        IEnumerable<GeneralDataPoint> GetAllBySectionName(string sectionName);
        // Получить все пункты общих указаний
        IEnumerable<GeneralDataPoint> GetAll();
        // Получить пункт общих указаний по id
        GeneralDataPoint GetById(int id);
        // Получить пункт общих указаний по unique key
        GeneralDataPoint GetByUniqueKey(
            int sectionId, string text);
    }
}
