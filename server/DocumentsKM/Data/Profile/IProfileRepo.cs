using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IProfileRepo
    {
        // Получить все профили по id вида профиля
        IEnumerable<Profile> GetAllByProfileClassId(int profileClassId);
        // Получить профиль по id
        Profile GetById(int id);
    }
}
