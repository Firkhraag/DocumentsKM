using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class MarkService : IMarkService
    {
        private IMarkRepo _repository;

        public MarkService(IMarkRepo MarkRepo)
        {
            _repository = MarkRepo;
        }

        public IEnumerable<Mark> GetAllBySubnodeId(int subnodeId)
        {
            return _repository.GetAllBySubnodeId(subnodeId);
        }

        public Mark GetById(int id)
        {
            return _repository.GetById(id);
        }
    }
}
