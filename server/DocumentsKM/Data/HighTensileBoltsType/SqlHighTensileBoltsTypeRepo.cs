using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlHighTensileBoltsTypeRepo : IHighTensileBoltsTypeRepo
    {
        private readonly ApplicationContext _context;

        public SqlHighTensileBoltsTypeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<HighTensileBoltsType> GetAll()
        {
            return _context.HighTensileBoltsTypes.ToList();
        }

        public HighTensileBoltsType GetById(int id)
        {
            return _context.HighTensileBoltsTypes.FirstOrDefault(t => t.Id == id);
        }
    }
}
