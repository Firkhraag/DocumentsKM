using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlLinkedDocTypeRepo : ILinkedDocTypeRepo
    {
        private readonly ApplicationContext _context;

        public SqlLinkedDocTypeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<LinkedDocType> GetAll()
        {
            return _context.LinkedDocTypes.ToList();
        }
    }
}
