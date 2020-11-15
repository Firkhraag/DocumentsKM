using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlSpecificationRepo : ISpecificationRepo
    {
        private readonly ApplicationContext _context;

        public SqlSpecificationRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Specification> GetAllByMarkId(int markId)
        {
            return _context.Specifications.Where(s => s.Mark.Id == markId).ToList();
        }

        public Specification GetById(int id)
        {
            return _context.Specifications.FirstOrDefault(m => m.Id == id);
        }

        public void Add(Specification specification)
        {
            _context.Specifications.Add(specification);
            _context.SaveChanges();
        }

        public void Update(Specification specification)
        {
            _context.Entry(specification).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Specification specification)
        {
            _context.Specifications.Remove(specification);
            _context.SaveChanges();
        }
    }
}
