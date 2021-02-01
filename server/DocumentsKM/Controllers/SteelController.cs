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
    public class SteelController : ControllerBase
    {
        private readonly ISteelService _service;

        public SteelController(
            ISteelService steelService)
        {
            _service = steelService;
        }

        [HttpGet, Route("steel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Steel>> GetAll()
        {
            var steel = _service.GetAll();
            return Ok(steel);
        }
    }
}
