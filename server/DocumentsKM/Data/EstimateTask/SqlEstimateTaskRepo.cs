using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlEstimateTaskRepo : IEstimateTaskRepo
    {
        private readonly ApplicationContext _context;

        public SqlEstimateTaskRepo(ApplicationContext context)
        {
            _context = context;
        }

        public EstimateTask GetByMarkId(int markId)
        {
            return _context.EstimateTask.SingleOrDefault(
                v => v.Mark.Id == markId);
        }

        public void Add(EstimateTask estimateTask)
        {
            _context.EstimateTask.Add(estimateTask);
            _context.SaveChanges();
        }

        public void Update(EstimateTask estimateTask)
        {
            _context.Entry(estimateTask).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
