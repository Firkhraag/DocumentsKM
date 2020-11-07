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
    public class ConstructionMaterialController : ControllerBase
    {
        private readonly IConstructionMaterialService _service;

        public ConstructionMaterialController(IConstructionMaterialService constructionMaterialService)
        {
            _service = constructionMaterialService;
        }

        [HttpGet, Route("construction-materials")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ConstructionMaterial>> GetAll()
        {
            var constructionMaterial = _service.GetAll();
            return Ok(constructionMaterial);
        }
    }
}
