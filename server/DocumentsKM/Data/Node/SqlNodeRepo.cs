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
            // return _context.Nodes.Include(
            //     v => v.Project).Include(
            //         v => v.ChiefEngineer).Where(
            //             v => v.Project.Id == projectId).ToList();
            return _context.Nodes.Where(v => v.Project.Id == projectId).ToList();
        }

        public Node GetById(int id)
        {
            // return _context.Nodes.Include(
            //     v => v.Project).Include(
            //         v => v.ChiefEngineer).SingleOrDefault(v => v.Id == id);
            return _context.Nodes.SingleOrDefault(v => v.Id == id);
        }
    }
}
