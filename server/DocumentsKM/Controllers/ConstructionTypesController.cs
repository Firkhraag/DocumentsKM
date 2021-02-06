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
    public class ConstructionTypesController : ControllerBase
    {
        private readonly IConstructionTypeService _service;

        public ConstructionTypesController(
            IConstructionTypeService constructionTypeService)
        {
            _service = constructionTypeService;
        }

        [HttpGet, Route("construction-types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ConstructionType>> GetAll()
        {
            var constructionType = _service.GetAll();
            return Ok(constructionType);
        }
    }
}
