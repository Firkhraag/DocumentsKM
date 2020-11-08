using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IHighTensileBoltsTypeRepo
    {
        // Получить все высокопрочные болты
        IEnumerable<HighTensileBoltsType> GetAll();
        // Получить высокопрочные болты по id
        HighTensileBoltsType GetById(int id);
    }
}
