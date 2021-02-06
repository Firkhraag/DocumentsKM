using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IAttachedDocRepo
    {
        // Получить все прочие прилагаемые документы по id марки
        IEnumerable<AttachedDoc> GetAllByMarkId(int markId);
        // Получить прочий прилагаемый документ по id
        AttachedDoc GetById(int id);
        // Получить прочий прилагаемый документ по unique key
        AttachedDoc GetByUniqueKey(int markId, string designation);
        // Добавить новый прочий прилагаемый документ
        void Add(AttachedDoc attachedDoc);
        // Изменить прочий прилагаемый документ
        void Update(AttachedDoc attachedDoc);
        // Удалить прочий прилагаемый документ
        void Delete(AttachedDoc attachedDoc);
    }
}
