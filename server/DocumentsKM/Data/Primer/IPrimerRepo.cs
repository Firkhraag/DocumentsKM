using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IPrimerRepo
    {
        // Получить всю грунтовку
        IEnumerable<Primer> GetAll();
    }
}
