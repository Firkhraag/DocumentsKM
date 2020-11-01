using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IEmployeeRepo
    {
        // Получить сотрудников по id  отдела
        IEnumerable<Employee> GetAllByDepartmentId(int departmentId);
        // Получить сотрудника по id
        Employee GetById(int id);
        // Получить всех сотрудников по id отдела с кодом должности из заданного интервала
        IEnumerable<Employee> GetAllByDepartmentIdAndPositions(int departmentId, int[] posIds);
        // Получить всех сотрудников по id отдела с заданным кодом должности
        IEnumerable<Employee> GetAllByDepartmentIdAndPosition(int departmentNumber, int posId);
    }
}
