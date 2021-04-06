using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlCorrProtCoatingRepo : ICorrProtCoatingRepo
    {
        private readonly ApplicationContext _context;

        public SqlCorrProtCoatingRepo(ApplicationContext context)
        {
            _context = context;
        }

        public CorrProtCoating GetByFastnessTypeAndGroup(
            int paintworkFastnessId,
            int paintworkTypeId,
            int paintworkGroup)
        {
           return _context.CorrProtCoatings.OrderByDescending(
                v => v.Priority).FirstOrDefault(v =>
                    v.PaintworkFastness.Id == paintworkFastnessId &&
                    v.PaintworkType.Id == paintworkTypeId &&
                    v.PaintworkGroup == paintworkGroup);
        }
    }
}
