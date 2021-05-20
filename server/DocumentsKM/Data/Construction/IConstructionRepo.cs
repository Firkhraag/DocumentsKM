using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IConstructionRepo
    {
        // Получить все виды конструкций по id выпуска спецификации
        IEnumerable<Construction> GetAllBySpecificationId(int specificationId);
        // Получить все виды конструкций по id марки
        IEnumerable<Construction> GetAllByMarkId(int markId);
        // Получить все виды конструкций включенных видов спецификаций по id марки
        IEnumerable<Construction> GetAllIncludedByMarkId(int markId);
        // Получить вид конструкции по id
        Construction GetById(int id, bool withEagerLoading = false);
        // Получить вид конструкции по unique key
        Construction GetByUniqueKey(int specificationId, string name, float paintWorkCoeff);
        // Добавить новый вид конструкции
        void Add(Construction construction);
        // Изменить вид конструкции
        void Update(Construction construction);
        // Удалить вид конструкции
        void Delete(Construction construction);
    }
}