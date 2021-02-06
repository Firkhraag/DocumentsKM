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
        private readonly IGeneralDataDocService _service;

        public GeneralDataDocController(
            IGeneralDataDocService generalDataDocService)
        {
            _service = generalDataDocService;
        }

        [HttpGet, Route("marks/{markId}/general-data")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetGeneralDataDocument(int markId)
        {
            try
            {
                var file = _service.GetDocByMarkId(markId);
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
    }
}
