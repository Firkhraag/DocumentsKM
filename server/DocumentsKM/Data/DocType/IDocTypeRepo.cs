using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IDocTypeRepo
    {
        // Получить тип документа по id
        DocType GetById(int id);
    }
}
