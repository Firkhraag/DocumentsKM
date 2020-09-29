using DocumentsKM.Dtos;
using DocumentsKM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentsKM.Services
{
    public interface IUserService
    {
        // Аутентификация
        Task<UserResponse> Authenticate(UserRequest user);
        // Обновление токена
        Task<UserResponse> RefreshToken(string token);
        // Отзыв токена
        Task<bool> RevokeToken(string token);
        // // Получить всех пользователей
        // IEnumerable<User> GetAll();
        // // Получить пользователя по id
        // User GetById(int id);
    }
}
