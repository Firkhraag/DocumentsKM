using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Services
{
    public interface IBoltDiameterService
    {
        // Получить все диаметры болтов
        IEnumerable<BoltDiameter> GetAll();
    }
}
