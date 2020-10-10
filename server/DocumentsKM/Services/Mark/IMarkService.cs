using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IMarkService
    {
        // Получить все марки по id подузла
        IEnumerable<Mark> GetAllBySubnodeId(int subnodeId);
        // Получить марку по id
        Mark GetById(int id);
        // Создать новую марку
        void Create(Mark mark,
            int subnodeId,
            int departmentNumber,
            int mainBuilderId,
            int chiefSpecialistId,
            int groupLeaderId);
        // Обновить существующую марку
        void Update(Mark mark,
            int subnodeId,
            int departmentNumber,
            int mainBuilderId,
            int chiefSpecialistId,
            int groupLeaderId);
    }
}
