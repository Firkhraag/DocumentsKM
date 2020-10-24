using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ISpecificationRepo
    {
        // Получить все выпуски спецификаций по id марки
        IEnumerable<Specification> GetAllByMarkId(int markId);
        // Получить выпуск спецификации по id
        Specification GetById(int id);
        // Добавить новый выпуск спецификации
        void Add(Specification specification);
        // Удалить выпуск спецификации
        void Delete(Specification specification);
    }
}
