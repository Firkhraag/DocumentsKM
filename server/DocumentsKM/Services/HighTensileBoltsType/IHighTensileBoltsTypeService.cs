using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IHighTensileBoltsTypeService
    {
        // Получить все высокопрочные болты
        IEnumerable<HighTensileBoltsType> GetAll();
    }
}
