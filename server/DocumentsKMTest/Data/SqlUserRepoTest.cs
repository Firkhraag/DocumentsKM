// using System.Linq;
// using DocumentsKM.Models;
// using DocumentsKM.Data;

// namespace DocumentsKM.Services
// {
//     public class SqlUserRepo : IUserRepo
//     {
//         private ApplicationContext _context;

//         public SqlUserRepo(ApplicationContext context)
//         {
//             _context = context;
//         }

//         // public IEnumerable<User> GetAll()
//         // {
//         //     return _context.Users;
//         // }

//         public User GetById(int id)
//         {
//             return _context.Users.Find(id);
//         }

//         public User GetByLogin(string login)
//         {
//             return _context.Users.SingleOrDefault(u => u.Login == login);
//         }
//     }
// }
