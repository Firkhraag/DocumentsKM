using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class ConstructionMaterialService : IConstructionMaterialService
    {
        private IConstructionMaterialRepo _repository;

        public ConstructionMaterialService(
            IConstructionMaterialRepo constructionMaterialRepo)
        {
            _repository = constructionMaterialRepo;
        }

        public IEnumerable<ConstructionMaterial> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
