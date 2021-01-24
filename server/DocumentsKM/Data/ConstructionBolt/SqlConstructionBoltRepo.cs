using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlConstructionBoltRepo : IConstructionBoltRepo
    {
        private readonly ApplicationContext _context;

        public SqlConstructionBoltRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ConstructionBolt> GetAllByConstructionId(
            int constructionId)
        {
            return _context.ConstructionBolts.Where(
                v => v.Construction.Id == constructionId).ToList();
        }

        public ConstructionBolt GetById(int id)
        {
            return _context.ConstructionBolts.SingleOrDefault(v => v.Id == id);
        }

        public ConstructionBolt GetByUniqueKey(
            int construtionId, int diameterId)
        {
            return _context.ConstructionBolts.SingleOrDefault(
                v => (v.Construction.Id == construtionId) &&
                    (v.Diameter.Id == diameterId));
        }

        public void Add(ConstructionBolt constructionBolt)
        {
            _context.ConstructionBolts.Add(constructionBolt);
            _context.SaveChanges();
        }

        public void Update(ConstructionBolt constructionBolt)
        {
            _context.Entry(constructionBolt).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(ConstructionBolt constructionBolt)
        {
            _context.ConstructionBolts.Remove(constructionBolt);
            _context.SaveChanges();
        }
    }
}
