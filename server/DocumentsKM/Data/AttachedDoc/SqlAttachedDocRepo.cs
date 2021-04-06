using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlAttachedDocRepo : IAttachedDocRepo
    {
        private readonly ApplicationContext _context;

        public SqlAttachedDocRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<AttachedDoc> GetAllByMarkId(int markId)
        {
            return _context.AttachedDocs.Where(
                v => v.Mark.Id == markId).OrderBy(v => v.Id).ToList();
        }

        public AttachedDoc GetById(int id)
        {
            return _context.AttachedDocs.SingleOrDefault(v => v.Id == id);
        }

        public AttachedDoc GetByUniqueKey(int markId, string designation)
        {
            return _context.AttachedDocs.SingleOrDefault(
                v => (v.Mark.Id == markId) && (v.Designation == designation));
        }

        public void Add(AttachedDoc attachedDoc)
        {
            _context.AttachedDocs.Add(attachedDoc);
            _context.SaveChanges();
        }

        public void Update(AttachedDoc attachedDoc)
        {
            _context.Entry(attachedDoc).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(AttachedDoc attachedDoc)
        {
            _context.AttachedDocs.Remove(attachedDoc);
            _context.SaveChanges();
        }
    }
}
