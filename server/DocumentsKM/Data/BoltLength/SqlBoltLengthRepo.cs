using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlBoltLengthRepo : IBoltLengthRepo
    {
        private readonly ApplicationContext _context;

        public SqlBoltLengthRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<BoltLength> GetAllByDiameterId(int diameterId)
        {
            return _context.BoltLengths.Where(v => v.Diameter.Id == diameterId).ToList();
        }

        public BoltLength GetById(int id)
        {
            return _context.BoltLengths.SingleOrDefault(v => v.Id == id);
        }
    }
}
