using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlGeneralDataPointRepo : IGeneralDataPointRepo
    {
        private readonly ApplicationContext _context;

        public SqlGeneralDataPointRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<GeneralDataPoint> GetAllBySectionId(
            int sectionId)
        {
            return _context.GeneralDataPoints.Where(
                v => v.Section.Id == sectionId).OrderBy(
                    v => v.OrderNum).ToList();
        }
        
        public IEnumerable<GeneralDataPoint> GetAllBySectionName(string sectionName)
        {
            return _context.GeneralDataPoints.Where(
                v => v.Section.Name == sectionName).OrderBy(
                    v => v.OrderNum).ToList();
        }

        public IEnumerable<GeneralDataPoint> GetAll()
        {
            return _context.GeneralDataPoints.OrderBy(
                    v => v.OrderNum).ToList();
        }

        public GeneralDataPoint GetById(int id)
        {
            return _context.GeneralDataPoints.SingleOrDefault(v => v.Id == id);
        }

        public GeneralDataPoint GetByUniqueKey(
            int sectionId, string text)
        {
            return _context.GeneralDataPoints.SingleOrDefault(
                v => v.Section.Id == sectionId && v.Text == text);
        }
    }
}
