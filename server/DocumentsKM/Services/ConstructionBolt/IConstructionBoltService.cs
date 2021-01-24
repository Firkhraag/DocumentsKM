using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IConstructionBoltService
    {
        // Получить все высокопрочные болты конструкции по id вида конструкции
        IEnumerable<ConstructionBolt> GetAllByConstructionId(int constructionId);
        // Добавить высокопрочный болт к конструкции
        void Create(
            ConstructionBolt constructionBolt,
            int constructionId,
            int boltDiameterId);
        // Изменить высокопрочный болт у конструкции
        void Update(int id, ConstructionBoltUpdateRequest constructionBoltRequest);
        // Удалить высокопрочный болт у конструкции
        void Delete(int id);
    }
}
