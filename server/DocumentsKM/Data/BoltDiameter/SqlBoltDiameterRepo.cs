using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlBoltDiameterRepo : IBoltDiameterRepo
    {
        private readonly ApplicationContext _context;

        public SqlBoltDiameterRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<BoltDiameter> GetAll()
        {
            return _context.BoltDiameters.ToList();
        }

        public BoltDiameter GetById(int id)
        {
            return _context.BoltDiameters.SingleOrDefault(v => v.Id == id);
        }
    }
}
