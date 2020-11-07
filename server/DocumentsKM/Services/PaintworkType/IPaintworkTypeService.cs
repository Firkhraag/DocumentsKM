using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IPaintworkTypeService
    {
        // Получить все типы лакокрасочного материала
        IEnumerable<PaintworkType> GetAll();
    }
}
