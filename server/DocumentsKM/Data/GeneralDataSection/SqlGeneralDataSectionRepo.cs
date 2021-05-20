using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

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
            return _context.GeneralDataSections.OrderBy(
                    v => v.OrderNum).ToList();
        }

        public GeneralDataSection GetById(int id, bool withEagerLoading = false)
        {
            if (withEagerLoading)
            {
                return _context.GeneralDataSections.AsSingleQuery().Include(
                    v => v.GeneralDataPoints).SingleOrDefault(
                        v => v.Id == id);
            }
            return _context.GeneralDataSections.SingleOrDefault(
                v => v.Id == id);
        }

        public GeneralDataSection GetByUniqueKey(string name)
        {
            return _context.GeneralDataSections.SingleOrDefault(
                v => v.Name == name);
        }
    }
}
