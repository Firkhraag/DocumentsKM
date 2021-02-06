using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlDocRepo : IDocRepo
    {
        private readonly ApplicationContext _context;

        public SqlDocRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Doc> GetAllByMarkId(int markId)
        {
            return _context.Docs.Where(
                v => v.Mark.Id == markId).OrderBy(
                    v => v.Type.Name).ThenBy(v => v.Num).ToList();
        }

        public IEnumerable<Doc> GetAllByMarkIdAndDocType(
            int markId, int docTypeId)
        {
            return _context.Docs.Where(
                v => (v.Mark.Id == markId) &&
                    (v.Type.Id == docTypeId)).ToList();
        }

        public IEnumerable<Doc> GetAllByMarkIdAndNotDocType(
            int markId, int docTypeId)
        {
            return _context.Docs.Where(
                v => (v.Mark.Id == markId) &&
                    (v.Type.Id != docTypeId)).ToList();
        }

        public Doc GetById(int id)
        {
            return _context.Docs.SingleOrDefault(d => d.Id == id);
        }

        public void Add(Doc doc)
        {
            _context.Docs.Add(doc);
            _context.SaveChanges();
        }

        public void Update(Doc doc)
        {
            _context.Entry(doc).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Doc doc)
        {
            _context.Docs.Remove(doc);
            _context.SaveChanges();
        }
    }
}
