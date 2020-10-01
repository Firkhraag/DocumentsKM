using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ISubnodeRepo
    {
        // Получить все подузлы по id узла
        IEnumerable<Subnode> GetAllByNodeId(int nodeId);

        // IEnumerable<Subnode> GetUserRecentSubnodes();

        // Получить подузел по id
        Subnode GetById(int id);
    }
}
