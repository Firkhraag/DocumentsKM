using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IArchiveService
    {
        IEnumerable<ArchiveProject> GetProjects(string param);

        IEnumerable<ArchiveNode> GetNodes(int projectId);

        IEnumerable<ArchiveNode> GetSubnodes(int nodeId);
    }
}
