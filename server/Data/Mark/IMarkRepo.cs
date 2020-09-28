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
        
        // IEnumerable<Mark> GetUserRecentMarks();

        // Получить марку по id
        Mark GetById(int id);

        void Create(Mark mark);
        void Update(Mark mark);

        // Применить изменения (EF)
        bool SaveChanges();
    }
}
