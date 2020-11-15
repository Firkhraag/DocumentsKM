using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlDocTypeRepo : IDocTypeRepo
    {
        private readonly ApplicationContext _context;

        public SqlDocTypeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public DocType GetById(int id)
        {
            return _context.DocTypes.FirstOrDefault(dt => dt.Id == id);
        }

        public IEnumerable<DocType> GetAllExceptId(int idToExclude)
        {
            return _context.DocTypes.Where(d => d.Id != idToExclude).ToList();
        }
    }
}
