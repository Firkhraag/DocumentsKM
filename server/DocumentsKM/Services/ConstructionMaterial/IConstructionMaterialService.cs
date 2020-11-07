using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IConstructionMaterialService
    {
        // Получить все материалы конструкций
        IEnumerable<ConstructionMaterial> GetAll();
    }
}
