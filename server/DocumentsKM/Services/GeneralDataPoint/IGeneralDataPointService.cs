using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IGeneralDataPointService
    {
        // Получить все пункты общих указаний по id пользователя и раздела
        IEnumerable<GeneralDataPoint> GetAllByUserAndSectionId(
            int userId, int sectionId);
        // Создать пункт общих указаний
        void Create(
            GeneralDataPoint generalDataPoint,
            int userId,
            int sectionId);
        // Обновить существующий пункт общих указаний
        void Update(int id, int userId, int sectionId, GeneralDataPointUpdateRequest generalDataPoint);
        // Удалить существующий пункт общих указаний
        void Delete(int id, int userId, int sectionId);
    }
}
