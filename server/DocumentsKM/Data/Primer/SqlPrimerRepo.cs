using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlPrimerRepo : IPrimerRepo
    {
        private readonly ApplicationContext _context;

        public SqlPrimerRepo(ApplicationContext context)
        {
            _context = context;
        }

        public Primer GetByGroup(int group)
        {
            return _context.Primer.FirstOrDefault(v =>
                v.GroupNum == group);
        }
    }
}
