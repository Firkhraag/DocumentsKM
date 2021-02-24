using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlStandardConstructionNameRepo : IStandardConstructionNameRepo
    {
        private readonly ApplicationContext _context;

        public SqlStandardConstructionNameRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<StandardConstructionName> GetAll()
        {
            return _context.StandardConstructionNames.ToList();
        }
    }
}
