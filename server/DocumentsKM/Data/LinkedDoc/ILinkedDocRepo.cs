using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ILinkedDocRepo
    {
        // Получить все ссылочные документы по типу
        IEnumerable<LinkedDoc> GetAllByDocTypeId(int docTypeId);
        // Получить ссылочный документ по id
        LinkedDoc GetById(int id);
    }
}
