using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class GasGroupService : IGasGroupService
    {
        private readonly IGasGroupRepo _repository;

        public GasGroupService(IGasGroupRepo gasGroupRepo)
        {
            _repository = gasGroupRepo;
        }

        public IEnumerable<GasGroup> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
