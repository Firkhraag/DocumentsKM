using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IAdditionalWorkRepo
    {
        // Получить все дополнительные работы по id марки
        IEnumerable<AdditionalWork> GetAllByMarkId(int markId);
        // Получить дополнительные работы по id
        AdditionalWork GetById(int id);
        // Получить дополнительные работы по unique key
        AdditionalWork GetByUniqueKey(int markId, int employeeId);
        // Добавить дополнительные работы
        void Add(AdditionalWork additionalWork);
        // Изменить дополнительные работы
        void Update(AdditionalWork additionalWork);
        // Удалить дополнительные работы
        void Delete(AdditionalWork additionalWork);
    }
}
