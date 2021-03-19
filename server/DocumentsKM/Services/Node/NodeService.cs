using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System.Linq;
using DocumentsKM.Dtos;

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

        public void UpdateAll(List<ArchiveNode> nodesFetched)
        {
            var nodes = _repository.GetAll();
            // Delete should be cascade if it's necessary
            // foreach (var node in nodes)
            // {
            //     if (!nodesFetched.Select(v => v.Id).Contains(node.Id))
            //         _repository.Delete(node);
            // }
            foreach (var nodeFetched in nodesFetched)
            {
                var foundNode = nodes.SingleOrDefault(v => v.Id == nodeFetched.Id);
                if (foundNode == null)
                    _repository.Add(nodeFetched.ToNode());
                else
                {
                    var wasChanged = false;
                    if (foundNode.Name != nodeFetched.Name)
                    {
                        foundNode.Name = nodeFetched.Name;
                        wasChanged = true;
                    }
                    if (wasChanged)
                        _repository.Update(foundNode);
                }
            }
        }
    }
}
