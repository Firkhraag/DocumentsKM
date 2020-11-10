using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IAttachedDocRepo
    {
        // Получить прочий прилагаемый документ по id
        AttachedDoc GetById(int id);
        // Получить прочий прилагаемый документ по марке и обозначению
        AttachedDoc GetByUniqueKeyValues(int markId, string designation);
        // Получить все прочие прилагаемые документы по id марки
        IEnumerable<AttachedDoc> GetAllByMarkId(int markId);
        // Добавить новый прочий прилагаемый документ
        void Add(AttachedDoc attachedDoc);
        // Изменить имеющийся прочий прилагаемый документ
        void Update(AttachedDoc attachedDoc);
        // Удалить имеющийся прочий прилагаемый документ
        void Delete(AttachedDoc attachedDoc);
    }
}
