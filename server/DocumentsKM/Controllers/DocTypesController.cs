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
    public class DocTypesController : ControllerBase
    {
        private readonly IDocTypeService _service;

        public DocTypesController(IDocTypeService docTypeService)
        {
            _service = docTypeService;
        }

        [HttpGet, Route("doc-types/attached")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DocType>> GetAllAttached()
        {
            var docTypes = _service.GetAllAttached();
            return Ok(docTypes);
        }
    }
}
