using System.Collections.Generic;
using Personnel.Models;

namespace Personnel.Data
{
    public interface IPositionRepo
    {
        IEnumerable<Position> GetAll();
        Position GetById(int id);
        void Add(Position position);
        void Update(Position position);
        void Delete(Position position);
    }
}
