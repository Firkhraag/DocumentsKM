using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IConstructionBoltRepo
    {
        // Получить все высокопрочные болты конструкции по id вида конструкции
        IEnumerable<ConstructionBolt> GetAllByConstructionId(int constructionId);
        // Получить высокопрочный болт конструкции по id
        ConstructionBolt GetById(int id);
        // Получить высокопрочный болт конструкции по unique key
        ConstructionBolt GetByUniqueKey(int construtionId, int diameterId);
        // Добавить высокопрочный болт к конструкции
        void Add(ConstructionBolt constructionBolt);
        // Изменить высокопрочный болт у конструкции
        void Update(ConstructionBolt constructionBolt);
        // Удалить высокопрочный болт у конструкции
        void Delete(ConstructionBolt constructionBolt);
    }
}
