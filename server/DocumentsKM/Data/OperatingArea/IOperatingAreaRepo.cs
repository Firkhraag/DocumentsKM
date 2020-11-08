using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IOperatingAreaRepo
    {
        // Получить все зоны эксплуатации
        IEnumerable<OperatingArea> GetAll();
        // Получить зону эксплуатации по id
        OperatingArea GetById(int id);
    }
}
