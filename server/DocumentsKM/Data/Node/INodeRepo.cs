using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface INodeRepo
    {
        IEnumerable<Node> GetAllByProjectId(int projectId);
        // Получить узел по id
        Node GetById(int id);
        // Получить узел по unique key
        Node GetByUniqueKey(int projectId, string code);
    }
}
