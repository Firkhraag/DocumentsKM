using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class NodeService : INodeService
    {
        private INodeRepo _repository;

        public NodeService(INodeRepo NodeRepo)
        {
            _repository = NodeRepo;
        }

        public IEnumerable<Node> GetAllByProjectId(int projectId)
        {
            return _repository.GetAllByProjectId(projectId);
        }
    }
}
