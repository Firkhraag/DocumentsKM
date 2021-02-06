using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkApprovalRepo
    {
        // Получить все согласования марки по id марки
        IEnumerable<MarkApproval> GetAllByMarkId(int markId);
        // Добавить согласование марки
        void Add(MarkApproval markApproval);
        // Удалить согласование марки
        void Delete(MarkApproval markApproval);
    }
}
