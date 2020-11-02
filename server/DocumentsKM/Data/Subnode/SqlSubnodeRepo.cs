using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlSubnodeRepo : ISubnodeRepo
    {
        private readonly ApplicationContext _context;

        public SqlSubnodeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Subnode> GetAllByNodeId(int nodeId)
        {
            return _context.Subnodes.Where(s => s.Node.Id == nodeId).ToList();
        }

        public Subnode GetById(int id)
        {
            return _context.Subnodes.FirstOrDefault(s => s.Id == id);
        }
    }
}