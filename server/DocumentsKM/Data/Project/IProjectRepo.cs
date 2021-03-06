using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IProjectRepo
    {
        // Получить все проекты
        IEnumerable<Project> GetAll();
        // Получить проект по id
        Project GetById(int id);
        // Получить проект по базовой серии
        Project GetByUniqueKey(string baseSeries);
        // Добавить проект
        void Add(Project project);
        // Обновить проект
        void Update(Project project);
        // Удалить проект
        void Delete(Project project);
    }
}
