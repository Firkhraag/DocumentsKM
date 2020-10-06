using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface ISubnodeService
    {
        // Получить все подузлы по id узла
        IEnumerable<Subnode> GetAllByNodeId(int nodeId);
        // Получить подузел по id
        Subnode GetById(int id);
    }
}
