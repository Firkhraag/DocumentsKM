using System.Linq;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentsKM.Data
{
    public class SqlDefaultValuesRepo : IDefaultValuesRepo
    {
        private readonly ApplicationContext _context;

        public SqlDefaultValuesRepo(ApplicationContext context)
        {
            _context = context;
        }

        public DefaultValues GetByUserId(int userId)
        {
            return _context.DefaultValues.SingleOrDefault(v => v.UserId == userId);
        }

        public void Update(DefaultValues defaultValues)
        {
            _context.Entry(defaultValues).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
