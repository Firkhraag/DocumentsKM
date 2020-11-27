using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IConstructionTypeService
    {
        // Получить все типы конструкций
        IEnumerable<ConstructionType> GetAll();
    }
}
