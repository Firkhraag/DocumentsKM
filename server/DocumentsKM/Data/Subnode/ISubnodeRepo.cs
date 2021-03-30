using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ISubnodeRepo
    {
        // Получить все подузлы по id узла
        IEnumerable<Subnode> GetAllByNodeId(int nodeId);
        // Получить подузел по id
        Subnode GetById(int id);
        // Получить подузел по unique key
        Subnode GetByUniqueKey(int nodeId, string code);
    }
}
