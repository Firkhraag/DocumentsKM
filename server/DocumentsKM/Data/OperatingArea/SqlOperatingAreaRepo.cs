using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlOperatingAreaRepo : IOperatingAreaRepo
    {
        private readonly ApplicationContext _context;

        public SqlOperatingAreaRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<OperatingArea> GetAll()
        {
            return _context.OperatingAreas.OrderBy(v => v.Id).ToList();
        }

        public OperatingArea GetById(int id)
        {
            return _context.OperatingAreas.SingleOrDefault(
                v => v.Id == id);
        }
    }
}
