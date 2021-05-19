using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IDepartmentRepo
    {
        // Получить все отделы
        IEnumerable<Department> GetAll();
        // Получить все активные отделы
        IEnumerable<Department> GetAllActive();
        // Получить отдел по id
        Department GetById(int id);
        // Получить отдел по коду
        Department GetByCode(string code);
        // Добавить отдел
        void Add(Department department);
        // Обновить отдел
        void Update(Department department);
        // Удалить отдел
        void Delete(Department department);
    }
}
