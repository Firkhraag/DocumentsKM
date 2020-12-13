using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlPaintworkTypeRepo : IPaintworkTypeRepo
    {
        private readonly ApplicationContext _context;

        public SqlPaintworkTypeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<PaintworkType> GetAll()
        {
            return _context.PaintworkTypes.ToList();
        }

        public PaintworkType GetById(int id)
        {
            return _context.PaintworkTypes.SingleOrDefault(v => v.Id == id);
        }
    }
}
