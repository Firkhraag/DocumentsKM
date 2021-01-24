using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface ILinkedDocService
    {
        // Получить все ссылочные документы по id типа
        IEnumerable<LinkedDoc> GetAllByDocTypeId(int docTypeId);
    }
}
