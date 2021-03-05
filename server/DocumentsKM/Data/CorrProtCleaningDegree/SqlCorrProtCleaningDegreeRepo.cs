using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlCorrProtCleaningDegreeRepo : ICorrProtCleaningDegreeRepo
    {
        private readonly ApplicationContext _context;

        public SqlCorrProtCleaningDegreeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<CorrProtCleaningDegree> GetAll()
        {
            return _context.CorrProtCleaningDegrees.ToList();
        }
    }
}
