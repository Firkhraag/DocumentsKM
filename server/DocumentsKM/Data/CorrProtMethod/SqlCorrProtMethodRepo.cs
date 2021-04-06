using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlCorrProtMethodRepo : ICorrProtMethodRepo
    {
        private readonly ApplicationContext _context;

        public SqlCorrProtMethodRepo(ApplicationContext context)
        {
            _context = context;
        }

        public CorrProtMethod GetByAggressivenessAndMaterialId(
            int envAggressivenessId,
            int constructionMaterialId)
        {
            return _context.CorrProtMethods.FirstOrDefault(
                v => v.EnvAggressiveness.Id == envAggressivenessId &&
                v.ConstructionMaterial.Id == constructionMaterialId);
        }
    }
}
