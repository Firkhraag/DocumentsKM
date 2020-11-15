using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class OperatingAreaService : IOperatingAreaService
    {
        private IOperatingAreaRepo _repository;

        public OperatingAreaService(IOperatingAreaRepo operatingAreaRepoo)
        {
            _repository = operatingAreaRepoo;
        }

        public IEnumerable<OperatingArea> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
