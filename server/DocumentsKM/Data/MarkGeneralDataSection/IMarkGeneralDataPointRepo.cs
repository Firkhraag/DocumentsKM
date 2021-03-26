using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkGeneralDataSectionRepo
    {
        // Получить все разделы общих указаний марки по id марки
        IEnumerable<MarkGeneralDataSection> GetAllByMarkId(int markId);
        // Получить раздел общих указаний марки по id
        MarkGeneralDataSection GetById(int id, bool withEagerLoading = false);
        // Получить раздел общих указаний марки по unique key
        MarkGeneralDataSection GetByUniqueKey(int markId, string name);
        // Добавить раздел общих указаний к марке
        void Add(MarkGeneralDataSection markGeneralDataSection);
        // Изменить раздел общих указаний у марки
        void Update(MarkGeneralDataSection markGeneralDataSection);
        // Удалить раздел общих указаний у марки
        void Delete(MarkGeneralDataSection markGeneralDataSection);
    }
}
