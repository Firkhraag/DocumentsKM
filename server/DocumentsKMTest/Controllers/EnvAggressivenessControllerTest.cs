using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsKM.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class EnvAggressivenessController : ControllerBase
    {
        private readonly IEnvAggressivenessService _service;

        public EnvAggressivenessController(IEnvAggressivenessService envAggressivenessService)
        {
            _service = envAggressivenessService;
        }

        [HttpGet, Route("env-aggressiveness")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EnvAggressiveness>> GetAll()
        {
            var envAggressiveness = _service.GetAll();
            return Ok(envAggressiveness);
        }
    }
}
