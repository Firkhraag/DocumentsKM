using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IMarkService
    {
        // Получить все марки по id подузла
        IEnumerable<Mark> GetAllBySubnodeId(int subnodeId);
        // Получить марку по id
        Mark GetById(int id);
        // Получить код для создания новой марки
        string GetNewMarkCode(int subnodeId);
        // Создать новую марку
        void Create(Mark mark,
            int userId,
            int subnodeId,
            int departmentId,
            int? chiefSpecialistId,
            int? groupLeaderId,
            int? normContrId);
        // Изменить марку
        void Update(int id, MarkUpdateRequest mark);
        // Обновить дату выдачи проекта на текущую
        void UpdateIssueDate(Mark mark);
        // Обновить дату выдачи проекта
        void UpdateIssueDate(Mark mark, DateTime date);
    }
}
