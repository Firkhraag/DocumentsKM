using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IProjectService
    {
        // Получить все проекты
        IEnumerable<Project> GetAll();
    }
}
