using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IEmployeeRepo
    {
        // Получить сотрудника по id
        Employee GetById(int id);
        // Получить всех сотрудников по номеру отдела с кодом должности из заданного интервала
        IEnumerable<Employee> GetAllByDepartmentNumberAndPositionRange(
            int departmentNumber, int minPosCode, int maxPosCode);
        // Получить всех сотрудников по номеру отдела с заданным кодом должности
        IEnumerable<Employee> GetAllByDepartmentNumberAndPosition(
            int departmentNumber, int posCode);
    }
}
