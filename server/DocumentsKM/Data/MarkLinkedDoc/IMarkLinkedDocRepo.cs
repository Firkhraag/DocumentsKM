using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkLinkedDocRepo
    {
        // Получить все согласования по id марки
        IEnumerable<MarkLinkedDoc> GetAllByMarkId(int markId);
        // Добавить ссылочный документ
        void Add(MarkLinkedDoc markLinkedDoc);
        // Удалить ссылочный документ
        void Delete(MarkLinkedDoc markLinkedDoc);
    }
}
