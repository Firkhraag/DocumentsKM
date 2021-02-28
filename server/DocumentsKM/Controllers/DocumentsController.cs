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

        [HttpGet, Route("marks/{markId}/general-data-document")]
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

        [HttpGet, Route("marks/{markId}/spec-document")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSpecificationDocument(int markId)
        {
            try
            {
                var file = _service.GetSpecificationDocument(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Спецификация металла.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet, Route("marks/{markId}/construction-document")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetConstructionDocument(int markId)
        {
            try
            {
                var file = _service.GetConstructionDocument(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Ведомость металлоконструкций.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet, Route("marks/{markId}/bolt-document")]
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

        [HttpGet, Route("marks/{markId}/estimate-task-document")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEstimateTaskDocument(int markId)
        {
            try
            {
                var file = _service.GetEstimateTaskDocument(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Задание на смету.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpGet, Route("marks/{markId}/project-reg")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProjectRegistrationDocument(int markId)
        {
            try
            {
                var file = _service.GetProjectRegistrationDocument(markId);
                return File(
                    file,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "Лист регистрации проекта.docx");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
