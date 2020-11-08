using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlEnvAggressivenessRepo : IEnvAggressivenessRepo
    {
        private readonly ApplicationContext _context;

        public SqlEnvAggressivenessRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<EnvAggressiveness> GetAll()
        {
            return _context.EnvAggressiveness.ToList();
        }

        public EnvAggressiveness GetById(int id)
        {
            return _context.EnvAggressiveness.FirstOrDefault(a => a.Id == id);
        }
    }
}
