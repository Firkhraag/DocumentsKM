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
    public class GasGroupController : ControllerBase
    {
        private readonly IGasGroupService _service;

        public GasGroupController(IGasGroupService gasGroupService)
        {
            _service = gasGroupService;
        }

        [HttpGet, Route("gas-groups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<GasGroup>> GetAll()
        {
            var gasGroup = _service.GetAll();
            return Ok(gasGroup);
        }
    }
}
