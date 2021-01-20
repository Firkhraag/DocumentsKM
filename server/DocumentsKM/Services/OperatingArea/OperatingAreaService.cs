using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class OperatingAreaService : IOperatingAreaService
    {
        private IOperatingAreaRepo _repository;

        public OperatingAreaService(IOperatingAreaRepo operatingAreaRepo)
        {
            _repository = operatingAreaRepo;
        }

        public IEnumerable<OperatingArea> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
