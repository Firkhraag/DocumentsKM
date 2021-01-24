using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IElementProfileRepo
    {
        // Получить все виды профилей для элемента конструкции
        IEnumerable<ElementProfile> GetAll();
        // Получить вид профиля для элемента конструкции по id
        ElementProfile GetById(int id);
    }
}
