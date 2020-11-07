using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlConstructionMaterialRepo : IConstructionMaterialRepo
    {
        private readonly ApplicationContext _context;

        public SqlConstructionMaterialRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ConstructionMaterial> GetAll()
        {
            return _context.ConstructionMaterials.ToList();
        }
    }
}
