using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class NodeService : INodeService
    {
        private readonly INodeRepo _repository;

        public NodeService(INodeRepo nodeRepo)
        {
            _repository = nodeRepo;
        }

        public IEnumerable<Node> GetAllByProjectId(int projectId)
        {
            return _repository.GetAllByProjectId(projectId);
        }

        public Node GetById(int id)
        {
            return _repository.GetById(id);
        }
    }
}
