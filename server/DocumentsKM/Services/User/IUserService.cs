using DocumentsKM.Dtos;
using System.Threading.Tasks;

namespace DocumentsKM.Services
{
    public interface IUserService
    {
        // Аутентифицировать пользователя
        Task<UserResponse> Authenticate(UserRequest user);
        // Обновить токен
        Task<UserResponse> RefreshToken(string token);
        // Отозвать токен
        Task<bool> RevokeToken(string token);
    }
}
