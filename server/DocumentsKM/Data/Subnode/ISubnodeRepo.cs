using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public interface ISubnodeRepo
    {
        IEnumerable<Subnode> GetAllNodeSubnodes(ulong nodeId);
        Subnode GetSubnodeById(ulong id);
    }
}
