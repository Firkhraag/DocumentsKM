using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface ISheetNameService
    {
        // Получить все типовые наименования листов
        IEnumerable<SheetName> GetAll();
    }
}
