using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ISheetNameRepo
    {
        // Получить все типовые наименования листа
        IEnumerable<SheetName> GetAll();
    }
}
