using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IAdditionalWorkRepo
    {
        // Получить прочий прилагаемый документ по id
        AdditionalWork GetById(int id);
        // Получить прочий прилагаемый документ по марке и обозначению
        AdditionalWork GetByUniqueKeyValues(int markId, int employeeId);
        // Получить все дополнительные работы по id марки
        IEnumerable<AdditionalWork> GetAllByMarkId(int markId);
        // Добавить дополнительные работы
        void Add(AdditionalWork additionalWork);
        // Изменить имеющийся прочий прилагаемый документ
        void Update(AdditionalWork additionalWork);
        // Удалить имеющийся прочий прилагаемый документ
        void Delete(AdditionalWork additionalWork);
    }
}
