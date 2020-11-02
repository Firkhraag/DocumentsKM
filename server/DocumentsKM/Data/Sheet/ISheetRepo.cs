using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ISheetRepo
    {
        // Получить лист по id
        Sheet GetById(int id);
        // Получить лист по id, номеру и типу документа
        Sheet GetByUniqueKeyValues(int markId, int num, int doctTypeId);
        // Получить все листы по id марки
        IEnumerable<Sheet> GetAllByMarkId(int markId);
        // Получить все листы по id марки и типу документа
        IEnumerable<Sheet> GetAllByMarkIdAndDocType(int markId, int docTypeId);
        // Добавить новый лист
        void Add(Sheet sheet);
        // Изменить имеющийся лист
        void Update(Sheet sheet);
        // Удалить имеющийся лист
        void Delete(Sheet sheet);
    }
}
