using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IDepartmentService
    {
        // Получить все активные отделы
        IEnumerable<Department> GetAll();
    }
}
