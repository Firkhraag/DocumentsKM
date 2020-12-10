using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IGeneralDataPointService
    {
        // Получить все пункты общих указаний по id раздела
        IEnumerable<GeneralDataPoint> GetAllByUserAndSectionId(
            int userId, int sectionId);
        // Создать пункт общих указаний
        void Create(
            GeneralDataPoint generalDataPoint,
            int userId,
            int sectionId);
        // Обновить существующий пункт общих указаний
        void Update(int id, GeneralDataPointUpdateRequest generalDataPoint);
        // Удалить существующий пункт общих указаний
        void Delete(int id);
    }
}
