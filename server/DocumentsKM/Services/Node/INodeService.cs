using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface INodeService
    {
        // Получить все узлы по id проекта
        IEnumerable<Node> GetAllByProjectId(int projectId);
        // Получить узел по id
        Node GetById(int id);
    }
}
