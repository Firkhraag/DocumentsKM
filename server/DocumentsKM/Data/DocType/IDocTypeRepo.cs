using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IDocTypeRepo
    {
        // Получить все типы документов кроме указанного id
        IEnumerable<DocType> GetAllExceptId(int idToExclude);
        // Получить тип документа по id
        DocType GetById(int id);
    }
}
