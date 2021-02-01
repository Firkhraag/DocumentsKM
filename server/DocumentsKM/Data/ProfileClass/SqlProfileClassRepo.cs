using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlProfileClassRepo : IProfileClassRepo
    {
        private readonly ApplicationContext _context;

        public SqlProfileClassRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ProfileClass> GetAll()
        {
            return _context.ProfileClasses.ToList();
        }

        public ProfileClass GetById(int id)
        {
            return _context.ProfileClasses.SingleOrDefault(v => v.Id == id);
        }
    }
}
