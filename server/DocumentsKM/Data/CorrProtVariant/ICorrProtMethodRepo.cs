using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ICorrProtMethodRepo
    {
        // Получить все методы антикоррозионной защиты
        IEnumerable<CorrProtMethod> GetAll();
    }
}
