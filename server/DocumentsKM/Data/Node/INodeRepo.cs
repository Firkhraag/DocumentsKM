using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface INodeRepo
    {
        // Получить все узлы
        IEnumerable<Node> GetAll();
        // Получить все узлы по id проекта
        IEnumerable<Node> GetAllByProjectId(int projectId);
        // Получить узел по id
        Node GetById(int id);
        // Получить узел по unique key
        Node GetByUniqueKey(int projectId, string code);
        // Добавить узел
        void Add(Node node);
        // Обновить узел
        void Update(Node node);
        // Удалить узел
        void Delete(Node node);
    }
}
