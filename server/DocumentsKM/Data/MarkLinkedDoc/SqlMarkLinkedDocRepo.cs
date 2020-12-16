using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

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
            // return _context.MarkLinkedDocs.Include(
            //         v => v.Mark).Include(
            //             v => v.LinkedDoc).Where(
            //                 v => v.Mark.Id == markId).ToList();
            return _context.MarkLinkedDocs.Where(
                v => v.Mark.Id == markId).ToList();
        }
        
        public MarkLinkedDoc GetById(int id)
        {
            // return _context.MarkLinkedDocs.Include(
            //         v => v.Mark).Include(
            //             v => v.LinkedDoc).SingleOrDefault(v => v.Id == id);
            return _context.MarkLinkedDocs.SingleOrDefault(v => v.Id == id);
        }

        public MarkLinkedDoc GetByMarkIdAndLinkedDocId(int markId, int linkedDocId)
        {
            // return _context.MarkLinkedDocs.Include(
            //         v => v.Mark).Include(
            //             v => v.LinkedDoc).SingleOrDefault(
            //                 v => v.Mark.Id == markId && v.LinkedDoc.Id == linkedDocId);
            return _context.MarkLinkedDocs.SingleOrDefault(
                v => v.Mark.Id == markId && v.LinkedDoc.Id == linkedDocId);
        }

        public void Add(MarkLinkedDoc markLinkedDoc)
        {
            _context.MarkLinkedDocs.Add(markLinkedDoc);
            _context.SaveChanges();
        }

        public void Update(MarkLinkedDoc markLinkedDoc)
        {
            _context.Entry(markLinkedDoc).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(MarkLinkedDoc markLinkedDoc)
        {
            _context.MarkLinkedDocs.Remove(markLinkedDoc);
            _context.SaveChanges();
        }
    }
}
