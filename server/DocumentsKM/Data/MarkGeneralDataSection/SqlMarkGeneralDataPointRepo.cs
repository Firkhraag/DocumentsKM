using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlMarkGeneralDataSectionRepo : IMarkGeneralDataSectionRepo
    {
        private readonly ApplicationContext _context;

        public SqlMarkGeneralDataSectionRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<MarkGeneralDataSection> GetAllByMarkId(int markId)
        {
            return _context.MarkGeneralDataSections.Where(
                v => v.Mark.Id == markId).OrderBy(
                        v => v.OrderNum).ToList();
        }

        public MarkGeneralDataSection GetById(int id, bool withEagerLoading = false)
        {
            if (withEagerLoading)
            {
                return _context.MarkGeneralDataSections.AsSingleQuery().Include(
                    v => v.MarkGeneralDataPoints).SingleOrDefault(
                        v => v.Id == id);
            }
            return _context.MarkGeneralDataSections.SingleOrDefault(
                v => v.Id == id);
        }

        public MarkGeneralDataSection GetByUniqueKey(
            int markId, string name)
        {
            return _context.MarkGeneralDataSections.SingleOrDefault(
                v => v.Mark.Id == markId && v.Name == name);
        }

        public void Add(MarkGeneralDataSection markGeneralDataSection)
        {
            _context.MarkGeneralDataSections.Add(markGeneralDataSection);
            _context.SaveChanges();
        }

        public void Update(MarkGeneralDataSection markGeneralDataSection)
        {
            _context.Entry(markGeneralDataSection).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(MarkGeneralDataSection markGeneralDataSection)
        {
            _context.MarkGeneralDataSections.Remove(markGeneralDataSection);
            _context.SaveChanges();
        }
    }
}
