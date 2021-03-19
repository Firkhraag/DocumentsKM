using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using System.Linq;

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

        public void UpdateAll(List<ArchiveNode> subnodesFetched)
        {
            var subnodes = _repository.GetAll();
            // Delete should be cascade if it's necessary
            // foreach (var subnode in subnodes)
            // {
            //     if (!subnodesFetched.Select(v => v.Id).Contains(subnode.Id))
            //         _repository.Delete(subnode);
            // }
            foreach (var subnodeFetched in subnodesFetched)
            {
                var foundSubnode = subnodes.SingleOrDefault(v => v.Id == subnodeFetched.Id);
                if (foundSubnode == null)
                    _repository.Add(subnodeFetched.ToSubnode());
                else
                {
                    var wasChanged = false;
                    if (foundSubnode.Name != subnodeFetched.Name)
                    {
                        foundSubnode.Name = subnodeFetched.Name;
                        wasChanged = true;
                    }
                    if (wasChanged)
                        _repository.Update(foundSubnode);
                }
            }
        }
    }
}
