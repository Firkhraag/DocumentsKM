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
    public class BoltDiametersController : ControllerBase
    {
        private readonly IBoltDiameterService _service;

        public BoltDiametersController(IBoltDiameterService boltDiameterService)
        {
            _service = boltDiameterService;
        }

        [HttpGet, Route("bolt-diameters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<BoltDiameter>> GetAll()
        {
            var boltDiameter = _service.GetAll();
            return Ok(boltDiameter);
        }
    }
}
