using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IDocRepo
    {
        // Получить все документы по id марки
        IEnumerable<Doc> GetAllByMarkId(int markId);
        // Получить все документы по марке и типу документа
        IEnumerable<Doc> GetAllByMarkIdAndDocType(
            int markId, int docTypeId);
        // Получить все документы по марке, кроме заданного типа документа
        IEnumerable<Doc> GetAllByMarkIdAndNotDocType(
            int markId, int docTypeId);
        // Получить документ по id
        Doc GetById(int id);
        // Добавить новый документ
        void Add(Doc doc);
        // Изменить документ
        void Update(Doc doc);
        // Удалить документ
        void Delete(Doc doc);
    }
}
