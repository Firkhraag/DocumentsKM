using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlSheetNameRepo : ISheetNameRepo
    {
        private readonly ApplicationContext _context;

        public SqlSheetNameRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<SheetName> GetAll()
        {
            return _context.SheetNames.ToList();
        }
    }
}
