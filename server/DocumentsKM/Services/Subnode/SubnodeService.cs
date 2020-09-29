using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class SubnodeService : ISubnodeService
    {
        private ISubnodeRepo _repository;

        public SubnodeService(ISubnodeRepo SubnodeRepo)
        {
            _repository = SubnodeRepo;
        }

        public IEnumerable<Subnode> GetAllByNodeId(int nodeId)
        {
            return _repository.GetAllByNodeId(nodeId);
        }
    }
}
