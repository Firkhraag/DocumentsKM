using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class SqlUserRepo : IUserRepo
    {
        private UserContext _context;

        public SqlUserRepo(UserContext context)
        {
            _context = context;
        }

        // public IEnumerable<User> GetAll()
        // {
        //     return _context.Users;
        // }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User GetByLogin(string login)
        {
            return _context.Users.SingleOrDefault(u => u.Login == login);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
