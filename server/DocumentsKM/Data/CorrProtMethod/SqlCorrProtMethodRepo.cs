using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlCorrProtMethodRepo : ICorrProtMethodRepo
    {
        private readonly ApplicationContext _context;

        public SqlCorrProtMethodRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<CorrProtMethod> GetAll()
        {
            return _context.CorrProtMethods.ToList();
        }
    }
}
