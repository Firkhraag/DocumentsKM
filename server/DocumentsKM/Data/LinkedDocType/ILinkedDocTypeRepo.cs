using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ILinkedDocTypeRepo
    {
        // Получить все типы ссылочных документов
        IEnumerable<LinkedDocType> GetAll();
    }
}
