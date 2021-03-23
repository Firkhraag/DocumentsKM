using Microsoft.AspNetCore.Mvc;
using DocumentsKM.Services;
using DocumentsKM.Dtos;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using System;

namespace DocumentsKM.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService userService)
        {
            _service = userService;
        }

        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserTokenResponse> Login([FromBody] UserRequest user)
        {
            var res = _service.Authenticate(user);
            if (res == null)
                return BadRequest(
                    new { message = "Неверный логин или пароль" });
            return Ok(res);
        }

        [Authorize]
        [HttpGet("me")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserResponse> GetUser()
        {
            var stringId = HttpContext.User.Identity.Name;
            if (stringId == null)
                return BadRequest();
            int id;
            try
            {
                id = int.Parse(stringId);
            }
            catch (FormatException)
            {
                return BadRequest();
            }
            var user = _service.GetUser(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
