using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IWeldingControlRepo
    {
        // Получить контроль сварки
        IEnumerable<WeldingControl> GetAll();
        // Получить контроль сварки по id
        WeldingControl GetById(int id);
    }
}
