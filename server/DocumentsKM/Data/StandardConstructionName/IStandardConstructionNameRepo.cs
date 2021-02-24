using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IStandardConstructionNameRepo
    {
        // Получить все типовые наименования типовых конструкций
        IEnumerable<StandardConstructionName> GetAll();
    }
}
