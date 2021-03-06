using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IDocService
    {
        // Получить все документы по id марки
        IEnumerable<Doc> GetAllByMarkId(int markId);
        // Получить все прилагаемые документы по id марки
        IEnumerable<Doc> GetAllAttachedByMarkId(int markId);
        // Получить все листы по id марки
        IEnumerable<Doc> GetAllSheetsByMarkId(int markId);
        // Создать новый документ
        void Create(
            Doc doc,
            int markId,
            int docTypeId,
            int creatorId,
            int? inspectorId,
            int? normContrId);
        // Изменить документ
        void Update(int id, DocUpdateRequest doc);
        // Удалить документ
        void Delete(int id);
    }
}
