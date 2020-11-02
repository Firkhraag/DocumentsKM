using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IMarkApprovalRepo
    {
        // Получить все согласования по id марки
        IEnumerable<MarkApproval> GetAllByMarkId(int markId);
        // Добавить исп
        void Add(MarkApproval markApproval);
        // Удалить исп
        void Delete(MarkApproval markApproval);
    }
}
