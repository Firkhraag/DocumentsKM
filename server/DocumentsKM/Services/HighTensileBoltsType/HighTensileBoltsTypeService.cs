using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class HighTensileBoltsTypeService : IHighTensileBoltsTypeService
    {
        private IHighTensileBoltsTypeRepo _repository;

        public HighTensileBoltsTypeService(
            IHighTensileBoltsTypeRepo highTensileBoltsTypeRepo)
        {
            _repository = highTensileBoltsTypeRepo;
        }

        public IEnumerable<HighTensileBoltsType> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
