using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IStandardConstructionService
    {
        // Получить все типовые конструкций по id выпуска спецификации
        IEnumerable<StandardConstruction> GetAllBySpecificationId(
            int specificationId);
        // Добавить типовую конструкцию
        void Create(
            StandardConstruction standardConstruction,
            int specificationId);
        // Изменить типовую конструкцию
        void Update(
            int id, StandardConstructionUpdateRequest standardConstruction);
        // Удалить типовую конструкцию
        void Delete(int id);
    }
}
