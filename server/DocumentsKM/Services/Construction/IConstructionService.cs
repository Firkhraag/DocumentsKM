using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IConstructionService
    {
        // Получить все виды конструкций по id выпуска спецификации
        IEnumerable<Construction> GetAllBySpecificationId(int specificationId);
        // Добавить вид конструкций
        void Create(
            Construction construction,
            int specificationId,
            int typeId,
            int? subtypeId,
            int weldingControlId);
        // Изменить вид конструкций
        void Update(int id, ConstructionUpdateRequest construction);
        // Удалить вид конструкций
        void Delete(int id);
    }
}
