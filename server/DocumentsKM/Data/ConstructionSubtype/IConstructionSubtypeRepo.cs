using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IConstructionSubtypeRepo
    {
        // Получить все подтипы конструкций по id типа конструкции
        IEnumerable<ConstructionSubtype> GetAllByTypeId(int typeId);
        // Получить подтип конструкци по id
        ConstructionSubtype GetById(int id);
    }
}
