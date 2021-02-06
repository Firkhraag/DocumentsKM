using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IProfileClassService
    {
        // Получить все виды профилей
        IEnumerable<ProfileClass> GetAll();
    }
}
