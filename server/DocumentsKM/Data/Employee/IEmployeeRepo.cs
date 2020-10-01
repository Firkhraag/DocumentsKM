using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IEmployeeRepo
    {
        // Получить сотрудника по id
        Employee GetById(int id);
        // Получить всех сотрудников по id отдела с кодом должности в интервале
        IEnumerable<Employee> GetAllByDepartmentNumberWithPositions(
            int departmentNumber, int minPosCode, int maxPosCode);
    }
}
