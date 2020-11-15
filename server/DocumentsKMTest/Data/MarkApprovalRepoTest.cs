using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlMarkApprovalRepo : IMarkApprovalRepo
    {
        private readonly ApplicationContext _context;

        public SqlMarkApprovalRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<MarkApproval> GetAllByMarkId(int markId)
        {
            return _context.MarkApprovals.Where(ma => ma.Mark.Id == markId).ToList();
        }

        public void Add(MarkApproval markApproval)
        {
            _context.MarkApprovals.Add(markApproval);
            _context.SaveChanges();
        }

        public void Delete(MarkApproval markApproval)
        {
            _context.MarkApprovals.Remove(markApproval);
            _context.SaveChanges();
        }
    }
}
