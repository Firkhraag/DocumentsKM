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
    public class ConstructionSubtypesController : ControllerBase
    {
        private readonly IConstructionSubtypeService _service;

        public ConstructionSubtypesController(IConstructionSubtypeService constructionSubtypeService)
        {
            _service = constructionSubtypeService;
        }

        [HttpGet, Route("construction-types/{typeId}/construction-subtypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ConstructionSubtype>> GetAllByTypeId(int typeId)
        {
            var constructionSubtype = _service.GetAllByTypeId(typeId);
            return Ok(constructionSubtype);
        }
    }
}
