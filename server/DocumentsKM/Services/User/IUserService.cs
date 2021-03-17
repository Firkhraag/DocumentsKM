using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public interface IUserService
    {
        // Аутентифицировать пользователя
        UserTokenResponse Authenticate(UserRequest user);
        // Получить пользователя
        UserResponse GetUser(int id);
    }
}
