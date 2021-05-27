using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IGeneralDataSectionService
    {
        // Получить все раздел общих указаний марки по id марки
        IEnumerable<GeneralDataSection> GetAll();
    }
}
