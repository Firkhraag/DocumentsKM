using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlWeldingControlRepo : IWeldingControlRepo
    {
        private readonly ApplicationContext _context;

        public SqlWeldingControlRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<WeldingControl> GetAll()
        {
            return _context.WeldingControl.ToList();
        }
    }
}
