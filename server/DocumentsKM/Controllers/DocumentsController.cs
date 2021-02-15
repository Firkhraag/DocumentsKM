using System;
using DocumentsKM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsKM.Controllers
{
    [Route("api")]
    [Authorize]
    [ApiController]
    public class GeneralDataDocController : ControllerBase
    {
        private readonly IDocumentService _service;

        public GeneralDataDocController(
            IDocumentService documentService)
        {
            _service = documentService;
        }

        [HttpGet, Route("marks/{markId}/general-data")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetGeneralDataDocument(int markId)
        {
            try
            {
                var file = _service.GetGeneralDataDocument(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Общие данные.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet, Route("marks/{markId}/bolts-doc")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBoltDocument(int markId)
        {
            try
            {
                var file = _service.GetBoltDocument(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Ведомость болтов.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
