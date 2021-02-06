using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IBoltDiameterRepo
    {
        // Получить все диаметры болтов
        IEnumerable<BoltDiameter> GetAll();
        // Получить диаметр болта по id
        BoltDiameter GetById(int id);
    }
}
