using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlProjectRepo : IProjectRepo
    {
        private readonly ApplicationContext _context;

        public SqlProjectRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Project> GetAll()
        {
            return _context.Projects.ToList();
        }

        public Project GetById(int id)
        {
            return _context.Projects.FirstOrDefault(p => p.Id == id);
        }
    }
}
