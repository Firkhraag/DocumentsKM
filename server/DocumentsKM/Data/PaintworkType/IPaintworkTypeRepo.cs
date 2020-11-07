using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IPaintworkTypeRepo
    {
        // Получить все типы лакокрасочного материала
        IEnumerable<PaintworkType> GetAll();
    }
}
