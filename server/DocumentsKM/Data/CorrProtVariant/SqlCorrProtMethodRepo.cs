using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlCorrProtVariantRepo : ICorrProtVariantRepo
    {
        private readonly ApplicationContext _context;

        public SqlCorrProtVariantRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<CorrProtVariant> GetAll()
        {
            return _context.CorrProtVariants.ToList();
        }
    }
}
