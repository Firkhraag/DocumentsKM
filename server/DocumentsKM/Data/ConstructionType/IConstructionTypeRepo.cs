using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IConstructionTypeRepo
    {
        // Получить все типы конструкций
        IEnumerable<ConstructionType> GetAll();
        // Получить тип конструкции по id
        ConstructionType GetById(int id);
    }
}
