using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IBoltLengthRepo
    {
        // Получить все длины болтов по id диаметра
        IEnumerable<BoltLength> GetAllByDiameterId(int diameterId);
        // Получить длину болта по id
        BoltLength GetById(int id);
    }
}
