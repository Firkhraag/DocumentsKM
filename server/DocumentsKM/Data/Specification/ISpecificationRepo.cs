using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ISpecificationRepo
    {
        // Получить все выпуски спецификаций по id марки
        IEnumerable<Specification> GetAllByMarkId(int markId);
        // Получить выпуск спецификации по id
        Specification GetById(int id, bool withEagerLoading = false);
        // Получить текущий выпуск спецификации по id марки
        Specification GetCurrentByMarkId(int markId);
        // Добавить новый выпуск спецификации
        void Add(Specification specification);
        // Изменить выпуск спецификации
        void Update(Specification specification);
        // Удалить выпуск спецификации
        void Delete(Specification specification);
    }
}
