using DocumentsKM.Models;
using System.Collections.Generic;

namespace DocumentsKM.Data
{
    public interface IUserRepo
    {
        // // Получить всех пользователей
        // IEnumerable<User> GetAll();

        // Получить пользователя по id
        User GetById(int id);
        // Получить пользователя по логину
        User GetByLogin(string login);
    }
}
