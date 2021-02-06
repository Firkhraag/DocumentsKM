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
    public class WeldingControlController : ControllerBase
    {
        private readonly IWeldingControlService _service;

        public WeldingControlController(
            IWeldingControlService weldingControlService)
        {
            _service = weldingControlService;
        }

        [HttpGet, Route("welding-control")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<WeldingControl>> GetAll()
        {
            var weldingControl = _service.GetAll();
            return Ok(weldingControl);
        }
    }
}
