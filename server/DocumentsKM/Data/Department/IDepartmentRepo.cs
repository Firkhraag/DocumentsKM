using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IDepartmentRepo
    {
        // Получить все отделы
        IEnumerable<Department> GetAll();
        // Получить отдел по id
        Department GetById(int id);
        // Добавить отдел
        void Add(Department department);
        // Обновить отдел
        void Update(Department department);
        // Удалить отдел
        void Delete(Department department);
    }
}
