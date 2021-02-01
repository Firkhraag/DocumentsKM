using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class SteelService : ISteelService
    {
        private readonly ISteelRepo _repository;

        public SteelService(ISteelRepo steelRepo)
        {
            _repository = steelRepo;
        }

        public IEnumerable<Steel> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
