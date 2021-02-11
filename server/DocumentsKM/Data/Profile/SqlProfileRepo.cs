using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlProfileRepo : IProfileRepo
    {
        private readonly ApplicationContext _context;

        public SqlProfileRepo(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Profile> GetAllByProfileClassId(int profileClassId)
        {
            return _context.Profiles.Where(
                v => v.Class.Id == profileClassId).ToList();
        }

        public Profile GetById(int id)
        {
            return _context.Profiles.SingleOrDefault(v => v.Id == id);
        }
    }
}
