using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DocumentsKM.Models;
using DocumentsKM.Dtos;
using DocumentsKM.Data;
using DocumentsKM.Helpers;
using System.Threading.Tasks;

namespace DocumentsKM.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _repository;
        private readonly AppSettings _appSettings;
        // private readonly ICacheService _cacheService;

        public UserService(
            IUserRepo userRepo,
            IOptions<AppSettings> appSettings)
            // ICacheService cacheService)
        {
            _repository = userRepo;
            _appSettings = appSettings.Value;
            // _cacheService = cacheService;
        }

        public async Task<UserResponse> Authenticate(UserRequest user)
        {
            // Ищем пользователя с данным логином
            var foundUser = _repository.GetByLogin(user.Login);
            // Если не найден, то возвращаем null
            if (foundUser == null) return null;
            // Проверяем пароль
            if (!BCrypt.Net.BCrypt.Verify(user.Password, foundUser.Password))
                return null;

            // Если все в порядке, создаем access и refresh tokens
            var accessToken = generateAccessToken(foundUser);
            var refreshToken = generateRefreshToken();
            // // Сохраняем refresh token в кэш
            // await _cacheService.SetCacheValueAsync(refreshToken, foundUser.Id.ToString(), _appSettings.TokensRedisDbNumber);
            return new UserResponse(foundUser.Id, foundUser.Employee, accessToken, refreshToken);
        }

        public async Task<UserResponse> RefreshToken(string token)
        {
            // // Ищем, имеется ли пользователь с данным токеном в хранилище
            // var idStr = await _cacheService.GetCacheValueAsync(token, _appSettings.TokensRedisDbNumber);
            // // Значение не найдено
            // if (idStr == null) return null;

            // int id;
            // try
            // {
            //     id = Int32.Parse(idStr);
            // }
            // catch (FormatException)
            // {
            //     return null;
            // }
            int id = 1;
            // Ищем пользователя
            var user = _repository.GetById(id);
            // Пользователь не найден
            if (user == null) return null;

            // Если успешно, то заменяем старый access token новым
            var accessToken = generateAccessToken(user);
            // return new UserResponse(user.Id, user.Employee.Name, accessToken, token);
            return new UserResponse(user.Id, user.Employee, accessToken, token);
        }

        public async Task<bool> RevokeToken(string token)
        {
            // // Ищем, имеется ли пользователь с данным токеном в хранилище
            // var idStr = await _cacheService.GetCacheValueAsync(token, _appSettings.TokensRedisDbNumber);
            // // Значение не найдено
            // if (idStr == null) return false;

            // int id;
            // try
            // {
            //     id = Int32.Parse(idStr);
            // }
            // catch (FormatException)
            // {
            //     return false;
            // }
            int id = 1;
            // Ищем пользователя
            var user = _repository.GetById(id);
            // Пользователь не найден
            if (user == null) return false;

            // // Если успешно, то удаляем refresh token
            // return await _cacheService.RemoveCacheKeyAsync(token, _appSettings.TokensRedisDbNumber);
            return true;
        }

        //------------------------HELPERS------------------------
        // Создание JWT access token
        private string generateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Создание refresh token для cookie
        private string generateRefreshToken()
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            // Заполняет массив байтов последовательностью случайных значений
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
