using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlElementProfileRepo : IElementProfileRepo
    {
        private readonly ApplicationContext _context;

        public SqlElementProfileRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ElementProfile> GetAll()
        {
            return _context.ElementProfiles.ToList();
        }

        public ElementProfile GetById(int id)
        {
            return _context.ElementProfiles.SingleOrDefault(
                v => v.Id == id);
        }
    }
}
