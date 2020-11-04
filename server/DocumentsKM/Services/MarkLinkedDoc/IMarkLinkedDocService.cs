using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IMarkLinkedDocService
    {
        // Получить все ссылочные документы по id марки
        IEnumerable<LinkedDoc> GetAllLinkedDocsByMarkId(int markId);
        // Обновить ссылочные документы у марки
        void Update(int markId, List<int> linkedDocIds);
    }
}
