using System.Linq;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public class SqlUserRepo : IUserRepo
    {
        private ApplicationContext _context;

        public SqlUserRepo(ApplicationContext context)
        {
            _context = context;
        }

        public User GetById(int id)
        {
            return _context.Users.SingleOrDefault(v => v.Id == id);
        }

        public User GetByLogin(string login)
        {
            return _context.Users.SingleOrDefault(v => v.Login == login);
        }
    }
}
