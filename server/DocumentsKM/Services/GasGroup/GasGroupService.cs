using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class GasGroupService : IGasGroupService
    {
        private IGasGroupRepo _repository;

        public GasGroupService(IGasGroupRepo gasGroupRepoo)
        {
            _repository = gasGroupRepoo;
        }

        public IEnumerable<GasGroup> GetAll()
        {
            return _repository.GetAll();
        }
    }
}