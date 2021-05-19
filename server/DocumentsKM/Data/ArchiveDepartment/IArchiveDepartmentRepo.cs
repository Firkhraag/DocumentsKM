using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IArchiveDepartmentRepo
    {
        // Получить отдел по id
        ArchiveDepartment GetById(int id);
    }
}
