using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class LinkedDocService : ILinkedDocService
    {
        private readonly ILinkedDocRepo _repository;

        public LinkedDocService(ILinkedDocRepo linkedDocRepo)
        {
            _repository = linkedDocRepo;
        }

        public IEnumerable<LinkedDoc> GetAllByDocTypeId(int docTypeId)
        {
            return _repository.GetAllByDocTypeId(docTypeId);
        }
    }
}
