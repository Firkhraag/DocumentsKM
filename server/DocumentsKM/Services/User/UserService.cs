using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DocumentsKM.Models;
using DocumentsKM.Dtos;
using DocumentsKM.Data;
using DocumentsKM.Helpers;

namespace DocumentsKM.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _repository;
        private readonly AppSettings _appSettings;

        public UserService(
            IUserRepo userRepo,
            IOptions<AppSettings> appSettings)
        {
            _repository = userRepo;
            _appSettings = appSettings.Value;
        }

        public UserTokenResponse Authenticate(UserRequest user)
        {
            var foundUser = _repository.GetByLogin(user.Login);
            if (foundUser == null) return null;
            if (user.Password != foundUser.Password)
                return null;

            var accessToken = generateToken(foundUser);
            return new UserTokenResponse(foundUser.Id, foundUser.Employee, accessToken);
        }

        public UserResponse GetUser(int id)
        {
            var user = _repository.GetById(id);
            if (user == null) return null;

            return new UserResponse(user.Id, user.Employee);
        }

        // Создание JWT access token
        private string generateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                // _appSettings.TokenExpireTimeInDays
                Expires = DateTime.UtcNow.AddDays(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
