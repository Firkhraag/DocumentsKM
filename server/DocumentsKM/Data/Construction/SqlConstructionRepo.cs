using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlConstructionRepo : IConstructionRepo
    {
        private readonly ApplicationContext _context;

        public SqlConstructionRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Construction> GetAllBySpecificationId(
            int specificationId)
        {
            return _context.Constructions.Where(
                v => v.Specification.Id == specificationId).OrderBy(
                    v => v.Type.Id).ToList();
        }

        public IEnumerable<Construction> GetAllByMarkId(
            int markId)
        {
            return _context.Constructions.Where(
                v => v.Specification.Mark.Id == markId).OrderBy(
                    v => v.Type.Id).ToList();
        }

        public Construction GetById(int id, bool withEagerLoading = false)
        {
            if (withEagerLoading)
            {
                return _context.Constructions.AsSingleQuery().Include(
                    v => v.ConstructionElements).Include(
                        v => v.ConstructionBolts).SingleOrDefault(
                            v => v.Id == id);
            }
            return _context.Constructions.SingleOrDefault(v => v.Id == id);
        }

        public Construction GetByUniqueKey(
            int specificationId, string name, float paintworkCoeff)
        {
            return _context.Constructions.SingleOrDefault(
                v => (v.Specification.Id == specificationId) &&
                    (v.Name == name) && (v.PaintworkCoeff == paintworkCoeff));
        }

        public void Add(Construction construction)
        {
            _context.Constructions.Add(construction);
            _context.SaveChanges();
        }

        public void Update(Construction construction)
        {
            _context.Entry(construction).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Construction construction)
        {
            _context.Constructions.Remove(construction);
            _context.SaveChanges();
        }
    }
}
