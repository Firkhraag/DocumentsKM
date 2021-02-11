using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IConstructionElementService
    {
        // Получить все элементы конструкции по id вида конструкции
        IEnumerable<ConstructionElement> GetAllByConstructionId(int constructionId);
        // Добавить элемент к конструкции
        void Create(
            ConstructionElement constructionElement,
            int constructionId,
            int profileTypeId,
            int steelId);
        // Изменить элемент у конструкции
        void Update(int id, ConstructionElementUpdateRequest constructionElementRequest);
        // Удалить элемент у конструкции
        void Delete(int id);
    }
}
