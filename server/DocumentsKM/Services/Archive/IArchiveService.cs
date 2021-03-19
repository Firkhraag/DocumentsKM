using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IArchiveService
    {
        IEnumerable<Project> GetProjects();

        IEnumerable<ArchiveNode> GetNodes();

        IEnumerable<ArchiveNode> GetSubnodes();
    }
}
