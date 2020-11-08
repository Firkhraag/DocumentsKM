using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IConstructionMaterialRepo
    {
        // Получить все материалы конструкций
        IEnumerable<ConstructionMaterial> GetAll();
        // Получить материал конструкций по id
        ConstructionMaterial GetById(int id);
    }
}
