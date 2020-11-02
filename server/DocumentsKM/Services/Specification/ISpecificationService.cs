using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface ISpecificationService
    {
        // Получить все выпуски спецификаций по id марки
        IEnumerable<Specification> GetAllByMarkId(int markId);
        // Создать новый выпуск спецификации
        Specification Create(int markId);
        // Обновить выпуск спецификации
        void Update(int markId, int id, SpecificationUpdateRequest specification);
        // Удалить выпуск спецификации
        void Delete(int markId, int id);
    }
}
