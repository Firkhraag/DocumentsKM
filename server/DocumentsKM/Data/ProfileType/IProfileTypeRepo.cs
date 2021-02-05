using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IProfileTypeRepo
    {
        // Получить все типы профилей
        IEnumerable<ProfileType> GetAll();
        // Получить тип профиля по id
        ProfileType GetById(int id);
    }
}
