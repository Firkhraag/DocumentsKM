using System;
using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    // Мок репозитория для тестирования
    public class MockSubnodeRepo : ISubnodeRepo
    {
        private readonly List<Subnode> _subnodes;

        public MockSubnodeRepo(INodeRepo nodeRepo)
        {
            // Начальные значения
            _subnodes = new List<Subnode>
            {
                new Subnode
                {
                    Id=0,
                    Node=nodeRepo.GetById(0),
                    Code="Test subnode1",
                },
                new Subnode
                {
                    Id=1,
                    Node=nodeRepo.GetById(0),
                    Code="Test subnode2",
                },
                new Subnode
                {
                    Id=2,
                    Node=nodeRepo.GetById(1),
                    Code="Test subnode3",
                },
            };
        }

        public IEnumerable<Subnode> GetAllByNodeId(int nodeId)
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
        
        public Subnode GetById(int id)
        {
            try
            {
                return _subnodes[id];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}
