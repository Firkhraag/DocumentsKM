using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Services
{
    public interface IPositionService
    {
        // Обновить должности
        void UpdateAll(List<Position> positionsFetched);
    }
}
