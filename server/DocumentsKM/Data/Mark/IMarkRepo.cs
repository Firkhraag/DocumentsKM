using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkRepo
    {
        // Получить все марки
        IEnumerable<Mark> GetAll();
        // Получить все марки по id подузла
        IEnumerable<Mark> GetAllBySubnodeId(int subnodeId);
        // Получить марку по id
        Mark GetById(int id);
        // Добавить новую марку
        void Add(Mark mark);
        // Изменить имеющуюся марку
        void Update(Mark mark);
    }
}
