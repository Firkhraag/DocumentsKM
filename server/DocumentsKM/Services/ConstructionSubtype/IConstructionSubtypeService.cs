using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IConstructionSubtypeService
    {
        // Получить все подтипы конструкций по id типа конструкции
        IEnumerable<ConstructionSubtype> GetAllByTypeId(int typeId);
    }
}
