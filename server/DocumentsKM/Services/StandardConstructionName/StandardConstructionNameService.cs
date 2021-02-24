using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class StandardConstructionNameService : IStandardConstructionNameService
    {
        private readonly IStandardConstructionNameRepo _repository;

        public StandardConstructionNameService(IStandardConstructionNameRepo standardConstructionNameRepo)
        {
            _repository = standardConstructionNameRepo;
        }

        public IEnumerable<StandardConstructionName> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
