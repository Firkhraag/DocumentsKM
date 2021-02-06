using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    public interface IUserRepo
    {
        // Получить пользователя по id
        User GetById(int id);
        // Получить пользователя по логину
        User GetByLogin(string login);
    }
}
