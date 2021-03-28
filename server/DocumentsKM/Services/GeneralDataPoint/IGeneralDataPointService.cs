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
        // Получить все пункты общих указаний по id пользователя и названию раздела
        IEnumerable<GeneralDataPoint> GetAllByUserIdAndSectionName(
            int userId, string sectionName);
        // Создать пункт общих указаний
        void Create(
            GeneralDataPoint generalDataPoint,
            int sectionId);
        // Изменить пункт общих указаний
        void Update(int id, int sectionId,
            GeneralDataPointUpdateRequest generalDataPoint);
        // Удалить пункт общих указаний
        void Delete(int id, int sectionId);
    }
}
