using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IWeldingControlService
    {
        // Получить контроль сварки
        IEnumerable<WeldingControl> GetAll();
    }
}
