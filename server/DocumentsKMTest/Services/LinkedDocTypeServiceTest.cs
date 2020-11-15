using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class LinkedDocTypeService : ILinkedDocTypeService
    {
        private ILinkedDocTypeRepo _repository;

        public LinkedDocTypeService(ILinkedDocTypeRepo linkedDocTypeRepoo)
        {
            _repository = linkedDocTypeRepoo;
        }

        public IEnumerable<LinkedDocType> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
