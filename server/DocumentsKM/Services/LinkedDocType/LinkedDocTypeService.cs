using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class LinkedDocTypeService : ILinkedDocTypeService
    {
        private readonly ILinkedDocTypeRepo _repository;

        public LinkedDocTypeService(ILinkedDocTypeRepo linkedDocTypeRepo)
        {
            _repository = linkedDocTypeRepo;
        }

        public IEnumerable<LinkedDocType> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
