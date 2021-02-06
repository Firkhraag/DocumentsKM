using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IProfileClassRepo
    {
        // Получить все виды профилей
        IEnumerable<ProfileClass> GetAll();
        // Получить вид профиля по id
        ProfileClass GetById(int id);
    }
}
