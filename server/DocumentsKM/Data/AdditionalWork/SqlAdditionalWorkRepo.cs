using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlAdditionalWorkRepo : IAdditionalWorkRepo
    {
        private readonly ApplicationContext _context;

        public SqlAdditionalWorkRepo(ApplicationContext context)
        {
            _context = context;
        }

        public AdditionalWork GetById(int id)
        {
            return _context.AdditionalWork.SingleOrDefault(v => v.Id == id);
        }

        public AdditionalWork GetByUniqueKeyValues(int markId, int employeeId)
        {
            return _context.AdditionalWork.SingleOrDefault(v => (v.Mark.Id == markId) &&
                (v.Employee.Id == employeeId));
        }

        public IEnumerable<AdditionalWork> GetAllByMarkId(int markId)
        {
            return _context.AdditionalWork.Where(v => v.Mark.Id == markId).ToList();
        }

        public void Add(AdditionalWork additionalWork)
        {
            _context.AdditionalWork.Add(additionalWork);
            _context.SaveChanges();
        }

        public void Update(AdditionalWork additionalWork)
        {
            _context.Entry(additionalWork).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(AdditionalWork additionalWork)
        {
            _context.AdditionalWork.Remove(additionalWork);
            _context.SaveChanges();
        }
    }
}
