using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface ILinkedDocTypeService
    {
        // Получить все типы ссылочных документов
        IEnumerable<LinkedDocType> GetAll();
    }
}
