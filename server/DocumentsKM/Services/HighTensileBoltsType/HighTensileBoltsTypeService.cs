using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class HighTensileBoltsTypeService : IHighTensileBoltsTypeService
    {
        private IHighTensileBoltsTypeRepo _repository;

        public HighTensileBoltsTypeService(IHighTensileBoltsTypeRepo highTensileBoltsTypeRepoo)
        {
            _repository = highTensileBoltsTypeRepoo;
        }

        public IEnumerable<HighTensileBoltsType> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
