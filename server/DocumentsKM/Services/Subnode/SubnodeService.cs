using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System.Linq;

namespace DocumentsKM.Services
{
    public class SubnodeService : ISubnodeService
    {
        private readonly ISubnodeRepo _repository;
        private readonly INodeRepo _nodeRepo;

        public SubnodeService(
            ISubnodeRepo subnodeRepo,
            INodeRepo nodeRepo)
        {
            _repository = subnodeRepo;
            _nodeRepo = nodeRepo;
        }

        public IEnumerable<Subnode> GetAllByNodeId(int nodeId)
        {
            return _repository.GetAllByNodeId(nodeId);
        }

        public Subnode GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void UpdateAll(List<Subnode> subnodesFetched)
        {
            var subnodes = _repository.GetAll();
            var nodeIds = _nodeRepo.GetAll().Select(v => v.Id);
            // Delete should be cascade if it's necessary
            // foreach (var subnode in subnodes)
            // {
            //     if (!subnodesFetched.Select(v => v.Id).Contains(subnode.Id))
            //         _repository.Delete(subnode);
            // }

            foreach (var subnodeFetched in subnodesFetched.Where(v => nodeIds.Contains(v.NodeId)))
            {
                var foundSubnode = subnodes.SingleOrDefault(v => v.Id == subnodeFetched.Id);
                if (foundSubnode == null)
                {
                    var uniqueKeyViolation = _repository.GetByUniqueKey(subnodeFetched.NodeId, subnodeFetched.Code);
                    if (uniqueKeyViolation == null)
                        _repository.Add(subnodeFetched);
                }
                else
                {
                    var wasChanged = false;
                    var uniqueKeyWasChanged = false;
                    var name = subnodeFetched.Name;
                    if (name != null)
                        name = name.Trim();
                    if (foundSubnode.Name != name)
                    {
                        foundSubnode.Name = name;
                        wasChanged = true;
                    }
                    var code = subnodeFetched.Code.Trim();
                    if (foundSubnode.Code != code)
                    {
                        foundSubnode.Code = code;
                        wasChanged = true;
                        uniqueKeyWasChanged = true;
                    }
                    if (foundSubnode.NodeId != subnodeFetched.NodeId)
                    {
                        foundSubnode.NodeId = subnodeFetched.NodeId;
                        wasChanged = true;
                        uniqueKeyWasChanged = true;
                    }
                    if (wasChanged)
                    {
                        if (uniqueKeyWasChanged)
                        {
                            var uniqueKeyViolation = _repository.GetByUniqueKey(subnodeFetched.NodeId, code);
                            if (uniqueKeyViolation != null)
                                return;
                        }
                        _repository.Update(foundSubnode);
                    }
                }
            }
        }
    }
}
