using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IDepartmentService
    {
        // Получить все отделы
        IEnumerable<Department> GetAll();
    }
}
