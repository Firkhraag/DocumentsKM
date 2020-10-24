using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface ISheetService
    {
        // Получить все листы по id марки
        IEnumerable<Sheet> GetAllByMarkId(int markId);
        // Создать новый лист
        void Create(int markId, string note);
    }
}
