using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface ISubnodeRepo
    {
        // Получить все подузлы по id узла
        IEnumerable<Subnode> GetAllByNodeId(int nodeId);
        // Получить подузел по id
        Subnode GetById(int id);
        // Добавить подузел
        void Add(Subnode subnode);
        // Обновить подузел
        void Update(Subnode subnode);
        // Удалить подузел
        void Delete(Subnode subnode);
    }
}
