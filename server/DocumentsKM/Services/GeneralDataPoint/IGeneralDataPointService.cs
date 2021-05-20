using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IGeneralDataPointService
    {
        // Получить все пункты общих указаний по id раздела
        IEnumerable<GeneralDataPoint> GetAllBySectionId(
            int sectionId);
        // Получить все пункты общих указаний по названию раздела
        IEnumerable<GeneralDataPoint> GetAllBySectionName(string sectionName);
    }
}
