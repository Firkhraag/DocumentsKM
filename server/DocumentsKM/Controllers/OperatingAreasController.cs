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
    public class OperatingAreasController : ControllerBase
    {
        private readonly IOperatingAreaService _service;

        public OperatingAreasController(IOperatingAreaService operatingAreaService)
        {
            _service = operatingAreaService;
        }

        [HttpGet, Route("operating-areas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OperatingArea>> GetAll()
        {
            var operatingArea = _service.GetAll();
            return Ok(operatingArea);
        }
    }
}
