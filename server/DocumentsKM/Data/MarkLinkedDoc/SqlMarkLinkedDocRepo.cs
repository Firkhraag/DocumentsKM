using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlMarkLinkedDocRepo : IMarkLinkedDocRepo
    {
        private readonly ApplicationContext _context;

        public SqlMarkLinkedDocRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<MarkLinkedDoc> GetAllByMarkId(int markId)
        {
            return _context.MarkLinkedDocs.Where(mld => mld.Mark.Id == markId).ToList();
        }

        public void Add(MarkLinkedDoc markLinkedDoc)
        {
            _context.MarkLinkedDocs.Add(markLinkedDoc);
            _context.SaveChanges();
        }

        public void Delete(MarkLinkedDoc markLinkedDoc)
        {
            _context.MarkLinkedDocs.Remove(markLinkedDoc);
            _context.SaveChanges();
        }
    }
}
