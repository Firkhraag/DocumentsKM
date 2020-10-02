using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IDepartmentService
    {
        // Получить все активные отделы
        IEnumerable<Department> GetAllActive();
        // Получить отдел по номеру
        Department GetByNumber(int number);
    }
}
