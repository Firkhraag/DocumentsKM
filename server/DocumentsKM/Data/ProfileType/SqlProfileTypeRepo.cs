using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlProfileTypeRepo : IProfileTypeRepo
    {
        private readonly ApplicationContext _context;

        public SqlProfileTypeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ProfileType> GetAll()
        {
            return _context.ProfileTypes.ToList();
        }

        public ProfileType GetById(int id)
        {
            return _context.ProfileTypes.SingleOrDefault(v => v.Id == id);
        }
    }
}
