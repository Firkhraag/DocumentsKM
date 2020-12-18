using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

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
            // return _context.Subnodes.Include(
            //     v => v.Node).Where(v => v.Node.Id == nodeId).ToList();
            return _context.Subnodes.Where(v => v.Node.Id == nodeId).ToList();
        }

        public Subnode GetById(int id)
        {
            return _context.Subnodes.Include(
                v => v.Node).SingleOrDefault(v => v.Id == id);
        }
    }
}