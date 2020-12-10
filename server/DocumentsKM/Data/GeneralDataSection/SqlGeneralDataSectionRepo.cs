using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlGeneralDataSectionRepo : IGeneralDataSectionRepo
    {
        private readonly ApplicationContext _context;

        public SqlGeneralDataSectionRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<GeneralDataSection> GetAll()
        {
            return _context.GeneralDataSections.OrderBy(v => v.OrderNum).ToList();
        }

        public GeneralDataSection GetById(int id)
        {
            return _context.GeneralDataSections.SingleOrDefault(v => v.Id == id);
        }
    }
}
