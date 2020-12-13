using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IGeneralDataSectionRepo
    {
        // Получить все разделы общих указаний
        IEnumerable<GeneralDataSection> GetAll();
        // Получить раздел общих указаний по id
        GeneralDataSection GetById(int id);
    }
}
