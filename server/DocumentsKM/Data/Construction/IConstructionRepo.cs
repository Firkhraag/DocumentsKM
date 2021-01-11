using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IConstructionRepo
    {
        // Получить вид конструкции по id
        Construction GetById(int id);
        // Получить вид конструкции по выпуску спецификации и обозначению
        Construction GetByUniqueKeyValues(int specificationId, string name, float paintWorkCoeff);
        // Получить все прочие прилагаемые документы по id марки
        IEnumerable<Construction> GetAllBySpecificationId(int specificationId);
        // Добавить новый вид конструкции
        void Add(Construction construction);
        // Изменить имеющийся вид конструкции
        void Update(Construction construction);
        // Удалить имеющийся вид конструкции
        void Delete(Construction construction);
    }
}
