using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System.Linq;

namespace DocumentsKM.Services
{
    public class NodeService : INodeService
    {
        private readonly INodeRepo _repository;
        private readonly IProjectRepo _projectRepo;

        public NodeService(INodeRepo nodeRepo,
            IProjectRepo projectRepo)
        {
            _repository = nodeRepo;
            _projectRepo = projectRepo;
        }

        public IEnumerable<Node> GetAllByProjectId(int projectId)
        {
            return _repository.GetAllByProjectId(projectId);
        }

        public void UpdateAll(List<Node> nodesFetched)
        {
            var nodes = _repository.GetAll();
            var projectIds = _projectRepo.GetAll().Select(v => v.Id);
            // Delete should be cascade if it's necessary
            // foreach (var node in nodes)
            // {
            //     if (!nodesFetched.Select(v => v.Id).Contains(node.Id))
            //         _repository.Delete(node);
            // }

            foreach (var nodeFetched in nodesFetched.Where(v => v.Code != null && projectIds.Contains(v.ProjectId)))
            {
                var foundNode = nodes.SingleOrDefault(v => v.Id == nodeFetched.Id);
                if (foundNode == null)
                {
                    var uniqueKeyViolation = _repository.GetByUniqueKey(nodeFetched.ProjectId, nodeFetched.Code);
                    if (uniqueKeyViolation == null)
                        _repository.Add(nodeFetched);
                }
                else
                {
                    var wasChanged = false;
                    var uniqueKeyWasChanged = false;
                    var name = nodeFetched.Name;
                    if (name != null)
                        name = name.Trim();
                    if (foundNode.Name != name)
                    {
                        foundNode.Name = name;
                        wasChanged = true;
                    }
                    var code = nodeFetched.Code.Trim();
                    if (foundNode.Code != code)
                    {
                        foundNode.Code = code;
                        wasChanged = true;
                        uniqueKeyWasChanged = true;
                    }
                    var chiefEngineer = nodeFetched.ChiefEngineer;
                    if (chiefEngineer != null)
                        chiefEngineer = chiefEngineer.Trim();
                    if (foundNode.ChiefEngineer != chiefEngineer)
                    {
                        foundNode.ChiefEngineer = chiefEngineer;
                        wasChanged = true;
                    }
                    if (foundNode.ProjectId != nodeFetched.ProjectId)
                    {
                        foundNode.ProjectId = nodeFetched.ProjectId;
                        wasChanged = true;
                        uniqueKeyWasChanged = true;
                    }
                    if (wasChanged)
                    {
                        if (uniqueKeyWasChanged)
                        {
                            var uniqueKeyViolation = _repository.GetByUniqueKey(nodeFetched.ProjectId, code);
                            if (uniqueKeyViolation != null)
                                return;
                        }
                        _repository.Update(foundNode);
                    }
                }
            }
        }
    }
}
