using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ICorrProtCleaningDegreeRepo
    {
        // Получить все степени очистки антикоррозионной защиты
        IEnumerable<CorrProtCleaningDegree> GetAll();
    }
}
