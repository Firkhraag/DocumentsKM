using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class ConstructionTypeService : IConstructionTypeService
    {
        private readonly IConstructionTypeRepo _repository;

        public ConstructionTypeService(
            IConstructionTypeRepo constructionTypeRepo)
        {
            _repository = constructionTypeRepo;
        }

        public IEnumerable<ConstructionType> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
