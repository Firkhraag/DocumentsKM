using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IDepartmentRepo
    {
        // Получить все активные отделы
        IEnumerable<Department> GetAllActive();
        // Получить отдел по номеру
        Department GetByNumber(int number);

        // Применить изменения (EF)
        bool SaveChanges();
    }
}
