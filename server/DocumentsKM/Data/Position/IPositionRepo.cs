using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IPositionRepo
    {
        // Получить все должности
        IEnumerable<Position> GetAll();
        // Получить должность по id
        Position GetById(int id);
        // Добавить должность
        void Add(Position position);
        // Обновить должность
        void Update(Position position);
        // Удалить должность
        void Delete(Position position);
    }
}
