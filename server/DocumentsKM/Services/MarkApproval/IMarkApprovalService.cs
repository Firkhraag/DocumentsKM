using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IMarkApprovalService
    {
        // Получить все согласования по id марки
        IEnumerable<Employee> GetAllByMarkId(int markId);
        // Обновить согласования у марки
        void Update(int markId, List<int> employeeIds);
    }
}
