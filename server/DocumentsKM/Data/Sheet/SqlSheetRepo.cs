using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlSheetRepo : ISheetRepo
    {
        private readonly ApplicationContext _context;

        public SqlSheetRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Sheet> GetAllByMarkId(int markId)
        {
            return _context.Sheets.Where(s => s.Mark.Id == markId).ToList();
        }

        public void Create(Sheet sheet)
        {
            _context.Sheets.Add(sheet);
            _context.SaveChanges();
        }
    }
}
