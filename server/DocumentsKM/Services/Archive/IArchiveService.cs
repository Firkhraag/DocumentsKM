using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IArchiveService
    {
        IEnumerable<Project> GetProjects();

        IEnumerable<Node> GetNodes();

        IEnumerable<Subnode> GetSubnodes();
    }
}
