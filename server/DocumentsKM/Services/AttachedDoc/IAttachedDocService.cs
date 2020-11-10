using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IAttachedDocService
    {
        // Получить все прочие прилагаемые документы по id марки
        IEnumerable<AttachedDoc> GetAllByMarkId(int markId);
        // Создать новый прочий прилагаемый документ
        void Create(
            AttachedDoc attachedDoc,
            int markId);
        // Обновить существующий прочий прилагаемый документ
        void Update(int id, AttachedDocUpdateRequest attachedDoc);
        // Удалить существующий прочий прилагаемый документ
        void Delete(int id);
    }
}
