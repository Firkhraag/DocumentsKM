using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IOperatingAreaService
    {
        // Получить все зоны эксплуатации
        IEnumerable<OperatingArea> GetAll();
    }
}
