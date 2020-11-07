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
    public class PaintworkTypeController : ControllerBase
    {
        private readonly IPaintworkTypeService _service;

        public PaintworkTypeController(IPaintworkTypeService paintworkTypeService)
        {
            _service = paintworkTypeService;
        }

        [HttpGet, Route("paintwork-types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PaintworkType>> GetAll()
        {
            var paintworkType = _service.GetAll();
            return Ok(paintworkType);
        }
    }
}
