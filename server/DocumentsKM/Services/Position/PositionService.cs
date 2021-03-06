using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System.Linq;

namespace DocumentsKM.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepo _repository;

        public PositionService(IPositionRepo positionRepo)
        {
            _repository = positionRepo;
        }

        public void UpdateAll(List<Position> positionsFetched)
        {
            var positions = _repository.GetAll();
            // Delete should be cascade if it's necessary
            // foreach (var position in positions)
            // {
            //     if (!positionsFetched.Select(v => v.Id).Contains(position.Id))
            //         _repository.Delete(position);
            // }
            foreach (var positionFetched in positionsFetched)
            {
                var foundPosition = positions.SingleOrDefault(v => v.Id == positionFetched.Id);
                if (foundPosition == null)
                    _repository.Add(positionFetched);
                else
                {
                    var name = positionFetched.Name.Trim();
                    if (foundPosition.Name != name)
                    {
                        foundPosition.Name = name;
                        _repository.Update(foundPosition);
                    }
                }
            }
        }
    }
}
