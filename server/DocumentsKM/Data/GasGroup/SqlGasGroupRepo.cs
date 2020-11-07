using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlGasGroupRepo : IGasGroupRepo
    {
        private readonly ApplicationContext _context;

        public SqlGasGroupRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<GasGroup> GetAll()
        {
            return _context.GasGroups.ToList();
        }
    }
}
