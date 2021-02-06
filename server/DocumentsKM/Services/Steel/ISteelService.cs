using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface ISteelService
    {
        // Получить все марки стали
        IEnumerable<Steel> GetAll();
    }
}
