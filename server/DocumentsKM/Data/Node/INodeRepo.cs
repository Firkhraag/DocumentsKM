using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public interface INodeRepo
    {
        IEnumerable<Node> GetAllProjectNodes(ulong projectId);
        Node GetNodeById(ulong id);
    }
}
