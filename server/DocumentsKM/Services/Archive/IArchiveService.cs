using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IArchiveService
    {
        IEnumerable<ArchiveProject> GetProjects();

        IEnumerable<ArchiveNode> GetNodes();

        IEnumerable<ArchiveNode> GetSubnodes();
    }
}
