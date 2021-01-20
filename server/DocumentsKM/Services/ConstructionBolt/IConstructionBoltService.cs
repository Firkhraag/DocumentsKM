using System.Collections.Generic;
using DocumentsKM.Dtos;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IConstructionBoltService
    {
        // Получить все высокопрочные болты по id вида конструкции
        IEnumerable<ConstructionBolt> GetAllByConstructionId(int constructionId);
        // Добавить высокопрочные болт к конструкции
        void Create(
            ConstructionBolt constructionBolt,
            int constructionId,
            int boltDiameterId);
        // Обновить высокопрочные болт у конструкции
        void Update(int id, ConstructionBoltUpdateRequest constructionBoltRequest);
        // Удалить высокопрочные болт у конструкции
        void Delete(int id);
    }
}
