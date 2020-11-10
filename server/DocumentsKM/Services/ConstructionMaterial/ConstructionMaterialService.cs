using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class ConstructionMaterialService : IConstructionMaterialService
    {
        private IConstructionMaterialRepo _repository;

        public ConstructionMaterialService(IConstructionMaterialRepo constructionMaterialRepoo)
        {
            _repository = constructionMaterialRepoo;
        }

        public IEnumerable<ConstructionMaterial> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
