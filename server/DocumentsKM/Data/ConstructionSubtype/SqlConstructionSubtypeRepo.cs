using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlConstructionSubtypeRepo : IConstructionSubtypeRepo
    {
        private readonly ApplicationContext _context;

        public SqlConstructionSubtypeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ConstructionSubtype> GetAllByTypeId(int typeId)
        {
            return _context.ConstructionSubtypes.Where(v => v.Type.Id == typeId).ToList();
        }

        public ConstructionSubtype GetById(int id)
        {
            return _context.ConstructionSubtypes.SingleOrDefault(v => v.Id == id);
        }
    }
}
