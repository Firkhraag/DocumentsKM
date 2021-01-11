using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IConstructionService
    {
        // Получить все виды конструкций по id выпуска спецификации
        IEnumerable<Construction> GetAllBySpecificationId(int specificationId);
        // Создать новый вид конструкций
        void Create(
            Construction construction,
            int specificationId,
            int typeId,
            int? subtypeId,
            int weldingControlId);
        // Обновить существующий вид конструкций
        void Update(int id, ConstructionUpdateRequest construction);
        // Удалить существующий вид конструкций
        void Delete(int id);
    }
}
