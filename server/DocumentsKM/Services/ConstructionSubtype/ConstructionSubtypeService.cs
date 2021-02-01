using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class ConstructionSubtypeService : IConstructionSubtypeService
    {
        private readonly IConstructionSubtypeRepo _repository;

        public ConstructionSubtypeService(
            IConstructionSubtypeRepo constructionSubtypeRepo)
        {
            _repository = constructionSubtypeRepo;
        }

        public IEnumerable<ConstructionSubtype> GetAllByTypeId(int typeId)
        {
            return _repository.GetAllByTypeId(typeId);
        }
    }
}
