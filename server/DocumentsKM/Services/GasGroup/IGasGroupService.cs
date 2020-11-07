using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IGasGroupService
    {
        // Получить все группы газов
        IEnumerable<GasGroup> GetAll();
    }
}
