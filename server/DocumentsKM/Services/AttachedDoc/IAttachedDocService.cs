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
        // Изменить прочий прилагаемый документ
        void Update(int id, AttachedDocUpdateRequest attachedDoc);
        // Удалить прочий прилагаемый документ
        void Delete(int id);
    }
}
