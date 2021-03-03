using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IConstructionElementRepo
    {
        // Получить все элементы конструкции по id вида конструкции
        IEnumerable<ConstructionElement> GetAllByConstructionId(int constructionId);
        // Получить все элементы конструкции по id выпуска спецификации
        IEnumerable<ConstructionElement> GetAllBySpecificationId(int specificationId);
        // Получить элемент конструкции по id
        ConstructionElement GetById(int id);
        // Добавить элемент к конструкции
        void Add(ConstructionElement constructionElement);
        // Изменить элемент у конструкции
        void Update(ConstructionElement constructionElement);
        // Удалить элемент у конструкции
        void Delete(ConstructionElement constructionElement);
    }
}
