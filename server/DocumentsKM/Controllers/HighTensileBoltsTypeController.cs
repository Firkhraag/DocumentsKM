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
    public class HighTensileBoltsTypeController : ControllerBase
    {
        private readonly IHighTensileBoltsTypeService _service;

        public HighTensileBoltsTypeController(IHighTensileBoltsTypeService highTensileBoltsTypeService)
        {
            _service = highTensileBoltsTypeService;
        }

        [HttpGet, Route("high-tensile-bolts-types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<HighTensileBoltsType>> GetAll()
        {
            var highTensileBoltsType = _service.GetAll();
            return Ok(highTensileBoltsType);
        }
    }
}
