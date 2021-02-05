using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IProfileService
    {
        // Получить все профили по id вида профиля
        IEnumerable<Profile> GetAllByProfileClass(int profileClassId);
    }
}
