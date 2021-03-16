using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using Serilog;
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
            Log.Information(positionsFetched.Count().ToString());
        }
    }
}
