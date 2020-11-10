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

        public AttachedDoc GetById(int id)
        {
            return _context.AttachedDocs.FirstOrDefault(d => d.Id == id);
        }

        public AttachedDoc GetByUniqueKeyValues(int markId, string designation)
        {
            return _context.AttachedDocs.FirstOrDefault(d => (d.Mark.Id == markId) &&
                (d.Designation == designation));
        }

        public IEnumerable<AttachedDoc> GetAllByMarkId(int markId)
        {
            return _context.AttachedDocs.Where(d => d.Mark.Id == markId).ToList();
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
