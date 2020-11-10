using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IDocRepo
    {
        // Получить документ по id
        Doc GetById(int id);
        // Получить документ по марке, номеру и типу документа
        Doc GetByUniqueKeyValues(int markId, int num, int docTypeId);
        // Получить все документы по id марки
        IEnumerable<Doc> GetAllByMarkId(int markId);
        // Получить все документы по марке и типу документа
        IEnumerable<Doc> GetAllByMarkIdAndDocType(int markId, int docTypeId);
        // Получить все документы по марке, кроме заданного типа документа
        IEnumerable<Doc> GetAllByMarkIdAndNotDocType(int markId, int docTypeId);
        // Добавить новый документ
        void Add(Doc doc);
        // Изменить имеющийся документ
        void Update(Doc doc);
        // Удалить имеющийся документ
        void Delete(Doc doc);
    }
}
