using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IEnvAggressivenessService
    {
        // Получить все агрессивности среды
        IEnumerable<EnvAggressiveness> GetAll();
    }
}
