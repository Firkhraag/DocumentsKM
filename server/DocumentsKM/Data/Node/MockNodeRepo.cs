using System;
using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    // Repo implementation for testing purposes
    public class MockNodeRepo : INodeRepo
    {
        // Initial Nodes list
        private readonly List<Node> _nodes;

        public MockNodeRepo(IProjectRepo projectRepo, IEmployeeRepo employeeRepo)
        {
            _nodes = new List<Node>
            {
                new Node
                {
                    Id=0,
                    Project=projectRepo.GetProjectById(0),
                    Code="111",
                    ChiefEngineer=employeeRepo.GetEmployeeById(0),
                },
                new Node
                {
                    Id=1,
                    Project=projectRepo.GetProjectById(0),
                    Code="222",
                    ChiefEngineer=employeeRepo.GetEmployeeById(1),
                },
                new Node
                {
                    Id=2,
                    Project=projectRepo.GetProjectById(1),
                    Code="333",
                    ChiefEngineer=employeeRepo.GetEmployeeById(2),
                },
            };
        }

        // GetAllProjectNodes returns the list of nodes for the project
        public IEnumerable<Node> GetAllProjectNodes(ulong projectId)
        {
            var nodesToReturn = new List<Node>();
            foreach (Node node in _nodes)
            {
                if (node.Project.Id == projectId)
                {
                    nodesToReturn.Add(node);
                }
            }
            if (nodesToReturn.Count == 0)
            {
                return null;
            }
            return nodesToReturn;
        }

        // GetNodeById returns the node with a given id
        public Node GetNodeById(ulong id)
        {
            try
            {
                var node = _nodes[Convert.ToInt32(id)];
                return node;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}
