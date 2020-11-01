using DocumentsKM.Dtos;
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
    }
}
