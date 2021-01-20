using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkLinkedDocRepo
    {
        // Получить все ссылочные документы по id марки
        IEnumerable<MarkLinkedDoc> GetAllByMarkId(int markId);
        // Получить ссылочный документ марки по id
        MarkLinkedDoc GetById(int id);
        // Получить ссылочный документ марки по id марки и id ссылочного документа
        MarkLinkedDoc GetByMarkIdAndLinkedDocId(int markId, int linkedDocId);
        // Добавить ссылочный документ к марке
        void Add(MarkLinkedDoc markLinkedDoc);
        // Обновить ссылочный документ у марки
        void Update(MarkLinkedDoc markLinkedDoc);
        // Удалить ссылочный документ у марки
        void Delete(MarkLinkedDoc markLinkedDoc);
    }
}
