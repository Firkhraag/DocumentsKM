using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface ISheetService
    {
        // Получить все листы по id марки
        IEnumerable<Sheet> GetAllByMarkId(int markId);
        // Получить все листы по id марки и типу документа
        IEnumerable<Sheet> GetAllByMarkIdAndDocTypeId(int markId, int docTypeId);
        // Создать новый лист
        void Create(
            Sheet sheet,
            int markId,
            int docTypeId,
            int? creatorId,
            int? inspectorId,
            int? normContrId);
        // Обновить существующий лист
        void Update(int id, SheetUpdateRequest sheet);
        // Удалить существующий лист
        void Delete(int id);
    }
}
