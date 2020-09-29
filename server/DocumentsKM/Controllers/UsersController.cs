using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DocumentsKM.Services;
using DocumentsKM.Dtos;
using System.Threading.Tasks;
using Serilog;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System;

namespace DocumentsKM.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _service;

        public UsersController(IUserService userService)
        {
            _service = userService;
        }

        // Аутентификация
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRequest user)
        {
            var res = await _service.Authenticate(user);
            if (res == null)
                return BadRequest(new { message = "Неверный логин или пароль" });
            setTokenCookie(res.RefreshToken, 7);
            return Ok(res);
        }

        // Обновление access token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();
            var res = await _service.RefreshToken(refreshToken);
            if (res == null)
                return Unauthorized(new { message = "Неверный токен" });
            return Ok(res);
        }

        // Выход из аккаунта
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            var token = Request.Cookies["refreshToken"];
            // Токен отсутствует
            if (string.IsNullOrEmpty(token))
                return NoContent();
            // Отзываем токен
            var res = await _service.RevokeToken(token);
            if (!res)
                return NoContent();
            setTokenCookie("", -1);
            return NoContent();
        }

        // // Получить всех пользователей системы
        // [Authorize]
        // [HttpGet]
        // public IActionResult GetAll()
        // {
        //     var users = _service.GetAll();
        //     return Ok(users);
        // }

        // // Получить конкретного пользователя
        // [Authorize]
        // [HttpGet("{id}")]
        // public IActionResult GetById(int id)
        // {
        //     var user = _service.GetById(id);
        //     if (user == null) return NotFound();
        //     return Ok(user);
        // }

        //------------------------HELPERS------------------------
        // Добавляет refresh token в cookie
        private void setTokenCookie(string token, int ttl)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(ttl)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
