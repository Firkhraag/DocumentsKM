using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlMarkGeneralDataPointRepo : IMarkGeneralDataPointRepo
    {
        private readonly ApplicationContext _context;

        public SqlMarkGeneralDataPointRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<MarkGeneralDataPoint> GetAllByMarkId(int markId)
        {
            return _context.MarkGeneralDataPoints.Where(
                v => v.Section.Mark.Id == markId).OrderBy(
                    v => v.Section.Id).ThenBy(
                        v => v.OrderNum).ToList();
        }

        public IEnumerable<MarkGeneralDataPoint> GetAllBySectionId(
            int sectionId)
        {
            return _context.MarkGeneralDataPoints.Where(
                v => v.Section.Id == sectionId).OrderBy(
                    v => v.OrderNum).ToList();
        }

        public MarkGeneralDataPoint GetById(int id)
        {
            return _context.MarkGeneralDataPoints.SingleOrDefault(v => v.Id == id);
        }

        public MarkGeneralDataPoint GetByUniqueKey(
            int sectionId, string text)
        {
            return _context.MarkGeneralDataPoints.SingleOrDefault(
                v => v.Section.Id == sectionId && v.Text == text);
        }

        public void Add(MarkGeneralDataPoint MarkgeneralDataPoint)
        {
            _context.MarkGeneralDataPoints.Add(MarkgeneralDataPoint);
            _context.SaveChanges();
        }

        public void Update(MarkGeneralDataPoint MarkgeneralDataPoint)
        {
            _context.Entry(MarkgeneralDataPoint).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(MarkGeneralDataPoint MarkgeneralDataPoint)
        {
            _context.MarkGeneralDataPoints.Remove(MarkgeneralDataPoint);
            _context.SaveChanges();
        }
    }
}
