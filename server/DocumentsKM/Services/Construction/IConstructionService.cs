using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IConstructionService
    {
        // Получить все виды конструкций по id выпуска спецификации
        IEnumerable<Construction> GetAllBySpecificationId(int specificationId);
        // Добавить вид конструкции
        void Create(
            Construction construction,
            int specificationId,
            int typeId,
            int? subtypeId,
            int weldingControlId);
        // Изменить вид конструкции
        void Update(int id, ConstructionUpdateRequest construction);
        // Удалить вид конструкции
        void Delete(int id);
        // Скопировать вид конструкции в выпуск спецификации
        void Copy(
            int constructionId,
            int specificationId);
    }
}
