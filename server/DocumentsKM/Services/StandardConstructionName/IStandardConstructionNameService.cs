using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IStandardConstructionNameService
    {
        // Получить все типовые наименования типовых конструкций
        IEnumerable<StandardConstructionName> GetAll();
    }
}
