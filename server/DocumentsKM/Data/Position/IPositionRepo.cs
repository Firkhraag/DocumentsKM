using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IPositionRepo
    {
        // Получить должность по коду
        Position GetByCode(int code);
    }
}
