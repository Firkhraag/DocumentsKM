using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IDepartmentRepo
    {
        // Получить все активные отделы
        IEnumerable<Department> GetAll();
        // Получить отдел по id
        Department GetById(int id);
    }
}
