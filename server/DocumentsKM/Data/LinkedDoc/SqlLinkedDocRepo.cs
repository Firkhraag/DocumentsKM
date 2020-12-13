using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlLinkedDocRepo : ILinkedDocRepo
    {
        private readonly ApplicationContext _context;

        public SqlLinkedDocRepo(ApplicationContext context)
        {
            _context = context;
        }

        public LinkedDoc GetById(int id)
        {
            return _context.LinkedDocs.SingleOrDefault(v => v.Id == id);
        }

        public IEnumerable<LinkedDoc> GetAllByDocTypeId(int docTypeId)
        {
            return _context.LinkedDocs.Where(v => v.Type.Id == docTypeId).ToList();
        }
    }
}
