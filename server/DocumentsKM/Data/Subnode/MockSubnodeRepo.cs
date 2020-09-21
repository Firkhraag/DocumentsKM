using System;
using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    // Repo implementation for testing purposes
    public class MockSubnodeRepo : ISubnodeRepo
    {
        // Initial Subnodes list
        private readonly List<Subnode> _subnodes;

        // Using the implementation of interface for testing
        public MockSubnodeRepo(INodeRepo nodeRepo)
        {
            _subnodes = new List<Subnode>
            {
                new Subnode
                {
                    Id=0,
                    Node=nodeRepo.GetNodeById(0),
                    Code="Test subnode1",
                },
                new Subnode
                {
                    Id=1,
                    Node=nodeRepo.GetNodeById(0),
                    Code="Test subnode2",
                },
                new Subnode
                {
                    Id=2,
                    Node=nodeRepo.GetNodeById(1),
                    Code="Test subnode3",
                },
            };
        }

        // GetAllNodeSubnodes returns the list of subnodes for the node
        public IEnumerable<Subnode> GetAllNodeSubnodes(ulong nodeId)
        {
            var subnodesToReturn = new List<Subnode>();
            foreach (Subnode subnode in _subnodes)
            {
                if (subnode.Node.Id == nodeId)
                {
                    subnodesToReturn.Add(subnode);
                }
            }
            return subnodesToReturn;
        }

        // // GetUserRecentSubnodes returns the list of subnodes where user recently created a mark (+or selected a mark???)
        // public IEnumerable<Subnode> GetUserRecentSubnodes()
        // {
        //     return _subnodes;
        // }
        
        // GetSubnodeById returns the subnode with a given id
        public Subnode GetSubnodeById(ulong id)
        {
            try
            {
                var subnode = _subnodes[Convert.ToInt32(id)];
                return subnode;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}
