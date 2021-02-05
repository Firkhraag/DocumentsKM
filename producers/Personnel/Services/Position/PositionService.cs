using System;
using System.Collections.Generic;
using Personnel.Data;
using Personnel.Dtos;
using Personnel.Models;

namespace Personnel.Services
{
    public class PositionService : IPositionService
    {
        private IPositionRepo _repository;

        public PositionService(IPositionRepo positionRepo)
        {
            _repository = positionRepo;
        }

        public IEnumerable<Position> GetAll()
        {
            return _repository.GetAll();
        }

        public void Create(Position position)
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));

            _repository.Add(position);
        }

        public void Update(
            int id,
            PositionRequest position)
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));
            var foundposition = _repository.GetById(id);
            if (foundposition == null)
                throw new ArgumentNullException(nameof(foundposition));

            foundposition.ShortName = position.ShortName;
            foundposition.LongName = position.LongName;
            _repository.Update(foundposition);
        }

        public void Delete(int id)
        {
            var foundposition = _repository.GetById(id);
            if (foundposition == null)
                throw new ArgumentNullException(nameof(foundposition));
            _repository.Delete(foundposition);
        }
    }
}
