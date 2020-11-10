using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IDocService
    {
        // Получить все прилагаемые документы по id марки
        IEnumerable<Doc> GetAllAttachedByMarkId(int markId);
        // Получить все листы по id марки
        IEnumerable<Doc> GetAllSheetsByMarkId(int markId);
        // Создать новый документ
        void Create(
            Doc doc,
            int markId,
            int docTypeId,
            int? creatorId,
            int? inspectorId,
            int? normContrId);
        // Обновить существующий документ
        void Update(int id, DocUpdateRequest doc);
        // Удалить существующий документ
        void Delete(int id);
    }
}
