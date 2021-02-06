using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class SubnodeService : ISubnodeService
    {
        private readonly ISubnodeRepo _repository;

        public SubnodeService(ISubnodeRepo subnodeRepo)
        {
            _repository = subnodeRepo;
        }

        public IEnumerable<Subnode> GetAllByNodeId(int nodeId)
        {
            return _repository.GetAllByNodeId(nodeId);
        }

        public Subnode GetById(int id)
        {
            return _repository.GetById(id);
        }
    }
}
