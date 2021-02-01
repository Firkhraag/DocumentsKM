using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlSteelRepo : ISteelRepo
    {
        private readonly ApplicationContext _context;

        public SqlSteelRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Steel> GetAll()
        {
            return _context.Steel.ToList();
        }

        public Steel GetById(int id)
        {
            return _context.Steel.SingleOrDefault(v => v.Id == id);
        }
    }
}
