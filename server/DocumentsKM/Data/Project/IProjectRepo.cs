using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public interface IProjectRepo
    {
        IEnumerable<Project> GetAllProjects();
        Project GetProjectById(ulong id);
    }
}
