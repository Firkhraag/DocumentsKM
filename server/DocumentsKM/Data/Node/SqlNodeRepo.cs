using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlNodeRepo : INodeRepo
    {
        private readonly ApplicationContext _context;

        public SqlNodeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Node> GetAllByProjectId(int projectId)
        {
            return _context.Nodes.Where(n => n.Project.Id == projectId);
        }

        public Node GetById(int id)
        {
            return _context.Nodes.FirstOrDefault(n => n.Id == id);
        }
    }
}
