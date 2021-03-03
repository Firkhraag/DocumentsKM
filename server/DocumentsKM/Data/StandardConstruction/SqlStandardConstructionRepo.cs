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

        public IEnumerable<StandardConstruction> GetAllByMarkId(int markId)
        {
            return _context.StandardConstructions.Where(
                v => v.Specification.Mark.Id == markId).ToList();
        }

        public StandardConstruction GetById(int id)
        {
            return _context.StandardConstructions.SingleOrDefault(v => v.Id == id);
        }

        public StandardConstruction GetByUniqueKey(int specificationId, string name)
        {
            return _context.StandardConstructions.SingleOrDefault(
                v => (v.Specification.Id == specificationId && v.Name == name));
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
