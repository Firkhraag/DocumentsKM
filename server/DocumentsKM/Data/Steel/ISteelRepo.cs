using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ISteelRepo
    {
        // Получить все марки стали
        IEnumerable<Steel> GetAll();
        // Получить марку стали по id
        Steel GetById(int id);
    }
}
