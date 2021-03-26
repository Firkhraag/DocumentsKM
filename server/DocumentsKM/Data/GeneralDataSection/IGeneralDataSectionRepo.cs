using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IGeneralDataSectionRepo
    {
        // Получить все разделы общих указаний марки по id пользователь
        IEnumerable<GeneralDataSection> GetAllByUserId(int userId);
        // Получить раздел общих указаний пользователь по id
        GeneralDataSection GetById(int id, bool withEagerLoading = false);
        // Получить раздел общих указаний пользователь по unique key
        GeneralDataSection GetByUniqueKey(int userId, string name);
        // Добавить раздел общих указаний к пользователю
        void Add(GeneralDataSection generalDataSection);
        // Изменить раздел общих указаний у пользователь
        void Update(GeneralDataSection generalDataSection);
        // Удалить раздел общих указаний у пользователь
        void Delete(GeneralDataSection generalDataSection);
    }
}
