using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IGeneralDataSectionService
    {
        // Получить все разделы общих указаний
        IEnumerable<GeneralDataSection> GetAll();
    }
}
