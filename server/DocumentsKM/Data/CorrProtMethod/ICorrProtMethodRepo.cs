using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ICorrProtVariantRepo
    {
        // Получить все варианты антикоррозионной защиты
        IEnumerable<CorrProtVariant> GetAll();
    }
}
