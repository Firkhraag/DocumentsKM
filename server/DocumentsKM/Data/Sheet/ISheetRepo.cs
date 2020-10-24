using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ISheetRepo
    {
        // Получить все листы по id марки
        IEnumerable<Sheet> GetAllByMarkId(int markId);
        // Создать новый лист
        void Create(Sheet sheet);
    }
}
