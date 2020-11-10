using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class LinkedDocService : ILinkedDocService
    {
        private ILinkedDocRepo _repository;

        public LinkedDocService(ILinkedDocRepo linkedDocRepoo)
        {
            _repository = linkedDocRepoo;
        }

        public IEnumerable<LinkedDoc> GetAllByDocTypeId(int docTypeId)
        {
            return _repository.GetAllByDocTypeId(docTypeId);
        }
    }
}
