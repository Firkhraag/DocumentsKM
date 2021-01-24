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
    public class LinkedDocsController : ControllerBase
    {
        private readonly ILinkedDocService _service;

        public LinkedDocsController(ILinkedDocService linkedDocService)
        {
            _service = linkedDocService;
        }

        [HttpGet, Route("linked-docs-types/{docTypeId}/docs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<LinkedDoc>> GetAllByDocTypeId(
            int docTypeId)
        {
            var linkedDocs = _service.GetAllByDocTypeId(docTypeId);
            return Ok(linkedDocs);
        }
    }
}
