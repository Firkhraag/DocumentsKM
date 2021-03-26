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

        public IEnumerable<GeneralDataSection> GetAllByUserId(int userId)
        {
            return _context.GeneralDataSections.Where(
                v => v.User.Id == userId).OrderBy(
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

        public GeneralDataSection GetByUniqueKey(
            int userId, string name)
        {
            return _context.GeneralDataSections.SingleOrDefault(
                v => v.User.Id == userId && v.Name == name);
        }

        public void Add(GeneralDataSection GeneralDataSection)
        {
            _context.GeneralDataSections.Add(GeneralDataSection);
            _context.SaveChanges();
        }

        public void Update(GeneralDataSection GeneralDataSection)
        {
            _context.Entry(GeneralDataSection).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(GeneralDataSection GeneralDataSection)
        {
            _context.GeneralDataSections.Remove(GeneralDataSection);
            _context.SaveChanges();
        }
    }
}
