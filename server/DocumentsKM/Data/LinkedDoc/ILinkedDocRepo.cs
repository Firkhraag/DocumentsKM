using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ILinkedDocRepo
    {
        // Получить ссылочный документ по id
        LinkedDoc GetById(int id);
        // Получить все ссылочные документы по типу
        IEnumerable<LinkedDoc> GetAllByDocTypeId(int docTypeId);
    }
}
