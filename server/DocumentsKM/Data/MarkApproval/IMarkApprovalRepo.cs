using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkApprovalRepo
    {
        // Получить все согласования по id марки
        IEnumerable<MarkApproval> GetAllByMarkId(int markId);
        // Добавить согласование
        void Add(MarkApproval markApproval);
        // Удалить согласование
        void Delete(MarkApproval markApproval);
    }
}
