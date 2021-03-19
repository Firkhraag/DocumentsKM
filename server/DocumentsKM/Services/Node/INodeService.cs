using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface INodeService
    {
        // Получить все узлы по id проекта
        IEnumerable<Node> GetAllByProjectId(int projectId);

        // Обновить все узлы
        void UpdateAll(List<ArchiveNode> nodesFetched);
    }
}
