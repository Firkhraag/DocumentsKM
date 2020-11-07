using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IEnvAggressivenessRepo
    {
        // Получить все агрессивности среды
        IEnumerable<EnvAggressiveness> GetAll();
    }
}
