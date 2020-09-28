using System;
using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    // Мок репозитория для тестирования
    public class MockNodeRepo : INodeRepo
    {
        private readonly List<Node> _nodes;

        public MockNodeRepo(IProjectRepo projectRepo, IEmployeeRepo employeeRepo)
        {
            // Начальные значения
            _nodes = new List<Node>
            {
                new Node
                {
                    Id=0,
                    Project=projectRepo.GetById(0),
                    Code="111",
                    ChiefEngineer=employeeRepo.GetById(0),
                },
                new Node
                {
                    Id=1,
                    Project=projectRepo.GetById(0),
                    Code="222",
                    ChiefEngineer=employeeRepo.GetById(1),
                },
                new Node
                {
                    Id=2,
                    Project=projectRepo.GetById(1),
                    Code="333",
                    ChiefEngineer=employeeRepo.GetById(2),
                },
            };
        }

        public IEnumerable<Node> GetAllByProjectId(int projectId)
        {
            var nodesToReturn = new List<Node>();
            foreach (Node node in _nodes)
            {
                if (node.Project.Id == projectId)
                    nodesToReturn.Add(node);
            }
            return nodesToReturn;
        }

        public Node GetById(int id)
        {
            try
            {
                return _nodes[id];
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
