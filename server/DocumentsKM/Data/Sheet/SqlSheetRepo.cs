using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlSheetRepo : ISheetRepo
    {
        private readonly ApplicationContext _context;

        public SqlSheetRepo(ApplicationContext context)
        {
            _context = context;
        }

        public Sheet GetById(int id)
        {
            return _context.Sheets.FirstOrDefault(s => s.Id == id);
        }

        public Sheet GetByUniqueKeyValues(int markId, int num, int doctTypeId)
        {
            return _context.Sheets.FirstOrDefault(s => (s.Mark.Id == markId) && (s.Num == num) && (s.DocType.Id == doctTypeId));
        }

        public IEnumerable<Sheet> GetAllByMarkId(int markId)
        {
            return _context.Sheets.Where(s => s.Mark.Id == markId).ToList();
        }

        public IEnumerable<Sheet> GetAllByMarkIdAndDocType(int markId, int docTypeId)
        {
            return _context.Sheets.Where(s => (s.Mark.Id == markId) && (s.DocType.Id == docTypeId)).ToList();
        }

        public void Add(Sheet sheet)
        {
            _context.Sheets.Add(sheet);
            _context.SaveChanges();
        }

        public void Update(Sheet sheet)
        {
            _context.Entry(sheet).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Sheet sheet)
        {
            _context.Sheets.Remove(sheet);
            _context.SaveChanges();
        }
    }
}
