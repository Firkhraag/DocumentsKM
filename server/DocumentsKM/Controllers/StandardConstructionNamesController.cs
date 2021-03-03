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
    public class StandardConstructionNamesController : ControllerBase
    {
        private readonly IStandardConstructionNameService _service;

        public StandardConstructionNamesController(IStandardConstructionNameService StandardConstructionNameService)
        {
            _service = StandardConstructionNameService;
        }

        [HttpGet, Route("standard-construction-names")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StandardConstructionName>> GetAll()
        {
            var StandardConstructionNames = _service.GetAll();
            return Ok(StandardConstructionNames);
        }
    }
}
