using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkRepo
    {
        // Получить все марки
        IEnumerable<Mark> GetAll();
        // Получить все марки по ids
        IEnumerable<Mark> GetAllByIds(List<int> ids);
        // Получить все марки по id подузла
        IEnumerable<Mark> GetAllBySubnodeId(int subnodeId);
        // Получить марку по id
        Mark GetById(int id);
        // Получить марку по unique key
        Mark GetByUniqueKey(int subnodeId, string code);
        // Добавить новую марку
        void Add(Mark mark);
        // Изменить имеющуюся марку
        void Update(Mark mark);
    }
}
