using Personnel.Models;

namespace Personnel.Data
{
    public interface IPositionRepo
    {
        Position GetById(int id);
    }
}
