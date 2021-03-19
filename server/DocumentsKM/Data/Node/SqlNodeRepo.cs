using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

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
            return _context.Nodes.Where(v => v.Project.Id == projectId).ToList();
        }

        public Node GetById(int id)
        {
            return _context.Nodes.SingleOrDefault(v => v.Id == id);
        }

        public void Add(Node node)
        {
            _context.Nodes.Add(node);
            _context.SaveChanges();
        }

        public void Update(Node node)
        {
            _context.Entry(node).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Node node)
        {
            _context.Nodes.Remove(node);
            _context.SaveChanges();
        }
    }
}
