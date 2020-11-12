using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IDocTypeService
    {
        // Получить все типы прилагаемых документов
        IEnumerable<DocType> GetAllAttached();
    }
}
