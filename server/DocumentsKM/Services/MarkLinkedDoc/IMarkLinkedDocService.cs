using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IMarkLinkedDocService
    {
        // Получить все ссылочные документы по id марки
        IEnumerable<MarkLinkedDoc> GetAllByMarkId(int markId);
        // Добавить ссылочный документ к марке
        void Create(
            MarkLinkedDoc markLinkedDoc,
            int markId,
            int linkedDocId);
        // Обновить ссылочный документ у марки
        void Update(int id, MarkLinkedDocRequest markLinkedDocRequest);
        // Удалить ссылочный документ у марки
        void Delete(int id);
    }
}
