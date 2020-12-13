using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlConstructionTypeRepo : IConstructionTypeRepo
    {
        private readonly ApplicationContext _context;

        public SqlConstructionTypeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ConstructionType> GetAll()
        {
            return _context.ConstructionTypes.ToList();
        }

        public ConstructionType GetById(int id)
        {
            return _context.ConstructionTypes.SingleOrDefault(v => v.Id == id);
        }
    }
}
