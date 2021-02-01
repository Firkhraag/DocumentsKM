using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IGasGroupRepo
    {
        // Получить все группы газов
        IEnumerable<GasGroup> GetAll();
        // Получить группу газов по id
        GasGroup GetById(int id);
    }
}
