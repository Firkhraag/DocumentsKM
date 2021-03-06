using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlConstructionElementRepo : IConstructionElementRepo
    {
        private readonly ApplicationContext _context;

        public SqlConstructionElementRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ConstructionElement> GetAllByConstructionId(
            int constructionId)
        {
            return _context.ConstructionElements.Where(
                v => v.Construction.Id == constructionId).OrderBy(
                    v => v.Id).ToList();
        }

        public IEnumerable<ConstructionElement> GetAllBySpecificationId(
            int specificationId)
        {
            return _context.ConstructionElements.Where(
                v => v.Construction.Specification.Id == specificationId).ToList();
        }

        public ConstructionElement GetById(int id)
        {
            return _context.ConstructionElements.SingleOrDefault(v => v.Id == id);
        }

        public void Add(ConstructionElement constructionElement)
        {
            _context.ConstructionElements.Add(constructionElement);
            _context.SaveChanges();
        }

        public void Update(ConstructionElement constructionElement)
        {
            _context.Entry(constructionElement).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(ConstructionElement constructionElement)
        {
            _context.ConstructionElements.Remove(constructionElement);
            _context.SaveChanges();
        }
    }
}
