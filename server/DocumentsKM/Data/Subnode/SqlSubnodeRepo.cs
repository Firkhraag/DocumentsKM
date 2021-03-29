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

        public IEnumerable<Subnode> GetAll()
        {
            return _context.Subnodes.ToList();
        }

        public IEnumerable<Subnode> GetAllByNodeId(int nodeId)
        {
            return _context.Subnodes.Where(
                v => v.Node.Id == nodeId).ToList();
        }

        public Subnode GetById(int id)
        {
            return _context.Subnodes.Include(
                v => v.Node).SingleOrDefault(v => v.Id == id);
        }

        public Subnode GetByUniqueKey(int nodeId, string code)
        {
            return _context.Subnodes.SingleOrDefault(
                v => v.NodeId == nodeId && v.Code == code);
        }

        public void Add(Subnode subnode)
        {
            _context.Subnodes.Add(subnode);
            _context.SaveChanges();
        }

        public void Update(Subnode subnode)
        {
            _context.Entry(subnode).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Subnode subnode)
        {
            _context.Subnodes.Remove(subnode);
            _context.SaveChanges();
        }
    }
}
