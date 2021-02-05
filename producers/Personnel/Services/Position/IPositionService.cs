using System.Collections.Generic;
using Personnel.Dtos;
using Personnel.Models;

namespace Personnel.Services
{
    public interface IPositionService
    {
        IEnumerable<Position> GetAll();
        void Create(Position position);
        void Update(int id, PositionRequest position);
        void Delete(int id);
    }
}
