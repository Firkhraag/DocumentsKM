using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlStandardConstructionRepo : IStandardConstructionRepo
    {
        private readonly ApplicationContext _context;

        public SqlStandardConstructionRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<StandardConstruction> GetAllBySpecificationId(int specificationId)
        {
            return _context.StandardConstructions.Where(
                v => v.Specification.Id == specificationId).ToList();
        }

        public StandardConstruction GetById(int id)
        {
            return _context.StandardConstructions.SingleOrDefault(v => v.Id == id);
        }

        public StandardConstruction GetByUniqueKey(int specificationId)
        {
            return _context.StandardConstructions.SingleOrDefault(
                v => (v.Specification.Id == specificationId));
        }

        public void Add(StandardConstruction standardconstruction)
        {
            _context.StandardConstructions.Add(standardconstruction);
            _context.SaveChanges();
        }

        public void Update(StandardConstruction standardconstruction)
        {
            _context.Entry(standardconstruction).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(StandardConstruction standardconstruction)
        {
            _context.StandardConstructions.Remove(standardconstruction);
            _context.SaveChanges();
        }
    }
}
