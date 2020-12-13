using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class NodeService : INodeService
    {
        private INodeRepo _repository;

        public NodeService(INodeRepo nodeRepo)
        {
            _repository = nodeRepo;
        }

        public IEnumerable<Node> GetAllByProjectId(int projectId)
        {
            return _repository.GetAllByProjectId(projectId);
        }
    }
}
