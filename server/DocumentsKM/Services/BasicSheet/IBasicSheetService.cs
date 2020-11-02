using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    // Листы основного комплекта
    public interface IBasicSheetService
    {
        // Получить все листы по id марки
        IEnumerable<Sheet> GetAllByMarkId(int markId);
        // Создать новый лист
        void Create(
            Sheet sheet,
            int markId,
            int? creatorId,
            int? inspectorId,
            int? normContrId);
        // Обновить существующий лист
        void Update(int id, SheetUpdateRequest sheet);
    }
}
